using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Common;
using Talabat.Core.Entities.Calender;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.AppointmentSpecs;
using Talabat.Core.Specifications.WorkScheduleSpecs;

namespace Talabat.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool CanCancelAppointment(Appointment appointment, DoctorPolicy policy)
        {
            var hoursUntilAppointment = (appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime) - DateTime.Now).TotalHours;

            if (!policy.AllowPatientCancellation)
                return false; // Cancellation not allowed at all

            if (hoursUntilAppointment < policy.MinCancellationHours)
                return policy.AllowLateCancellationReschedule; // Allow reschedule instead?

            return true;
        }

        public bool CanRescheduleAppointment(Appointment appointment, DoctorPolicy policy)
        {
            if (!policy.AllowRescheduling)
                return false; // Rescheduling not allowed at all

            if (appointment.RescheduleCount >= policy.MaxRescheduleAttempts)
                return false; // Exceeded max reschedule attempts

            var hoursUntilAppointment = (appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime) - DateTime.Now).TotalHours;
            return hoursUntilAppointment >= policy.MinRescheduleHours;
        }

        public static List<TimeOnly> GenerateTimeSlots(WorkSchedule schedule, int slotDurationMinutes)
        {
            var slots = new List<TimeOnly>();
            TimeOnly currentSlot = schedule.StartTime; // ✅ Keep the original StartTime untouched

            while (currentSlot < schedule.EndTime)
            {
                slots.Add(currentSlot);
                currentSlot = currentSlot.AddMinutes(slotDurationMinutes); // ✅ Modify only the local variable
            }

            return slots;
        }

        public async Task<ServiceResult<Dictionary<DateOnly, List<TimeOnly>>>> GetAvailableSlotsAsync(Doctor doctor)
        {

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            var appointmentSpec = new AppointmentSpecifications(doctor.Id, today);
            var bookedAppointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(appointmentSpec);

            if (doctor.WorkSchedules.IsNullOrEmpty())   
            {
                return ServiceResult<Dictionary<DateOnly, List<TimeOnly>>>.Failure("No work schedules found for this doctor.");
            }


            // Use schedule exceptions from the doctor entity and handle if Null
            var scheduleExceptions = doctor.ScheduleExceptions?.ToDictionary(e => e.Date, e => e) ?? new Dictionary<DateOnly, ScheduleException>();

            // Convert work schedules to a dictionary for fast lookup
            var workScheduleDict = doctor.WorkSchedules.ToDictionary(s => s.Day, s => s);

            // Group booked appointments by date
            var bookedAppointmentsDict = bookedAppointments
                .GroupBy(a => a.AppointmentDate)
                .ToDictionary(g => g.Key, g => g.ToList());


            // Initialize available slots dictionary
            Dictionary<DateOnly, List<TimeOnly>> availableSlots = new();
            _unitOfWork.Repository<Doctor>().Detach(doctor);

            // Loop through the whole month
            for (int i = 0; i < daysInMonth; i++)
            {
                DateOnly currentDate = today.AddDays(i);
                DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;

                // 1️⃣ Check if there's an exception for this specific date
                if (scheduleExceptions.TryGetValue(currentDate, out var exception))
                {
                    if (!exception.IsAvailable)
                    {
                        continue; // Doctor is unavailable on this day
                    }

                    // Generate time slots based on exception
                    var exceptionSlots = GenerateTimeSlots(new WorkSchedule
                    {
                        StartTime = exception.StartTime.Value,
                        EndTime = exception.EndTime.Value,
                        Day = currentDayOfWeek
                    }, doctor.SlotDurationMinutes);

                    // Remove booked slots
                    if (bookedAppointmentsDict.TryGetValue(currentDate, out var bookedAppointmentsForDay))
                    {
                        // Get the times for all booked appointments, including cancelled ones
                        var bookedAndCancelledTimesForDay = bookedAppointmentsForDay
                            .Where(a => a.Status == AppointmentStatus.Confirmed ||
                                        a.Status == AppointmentStatus.Pending)
                                        //we should remove the cancelled
                            .Select(a => a.AppointmentTime)  // We need to compare AppointmentTime (TimeOnly)
                            .ToList();

                        // Remove booked/cancelled times from the generated exception slots
                        exceptionSlots = exceptionSlots.Except(bookedAndCancelledTimesForDay).ToList();
                    }

                    if (exceptionSlots.Any())
                    {
                        availableSlots[currentDate] = exceptionSlots;
                    }

                    continue; // Move to the next day
                }


                // 2️⃣ No exception → use default schedule

                if (workScheduleDict.TryGetValue(currentDayOfWeek, out var schedule))
                {
                    Console.WriteLine($"[DEBUG] Found schedule for {currentDayOfWeek}: {schedule.StartTime} - {schedule.EndTime}");

                    var slots = GenerateTimeSlots(schedule, doctor.SlotDurationMinutes);

                    // Remove booked slots
                    if (bookedAppointmentsDict.TryGetValue(currentDate, out var bookedAppointmentsForDay))
                    {
                        // Get the times for all booked appointments, including cancelled ones
                        var bookedAndCancelledTimesForDay = bookedAppointmentsForDay
                            .Where(a => a.Status == AppointmentStatus.Confirmed)
                            .Select(a => a.AppointmentTime)  // We need to compare AppointmentTime (TimeOnly)
                            .ToList();

                        // Remove booked/cancelled times from the generated slots
                        slots = slots.Except(bookedAndCancelledTimesForDay).ToList();
                    }

                    if (slots.Any())
                    {
                        availableSlots[currentDate] = slots;
                    }
                }
            }


            return ServiceResult<Dictionary<DateOnly, List<TimeOnly>>>.Success(availableSlots);
        }

    }
}
