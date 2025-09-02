using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.Core;
using Talabat.Core.Entities.Calender;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.AppointmentSpecs;
using Talabat.Core.Specifications.DoctorPolicySpecs;
using Talabat.Core.Specifications.WorkScheduleSpecs;
using Talabat.Core.Common;
using Talabat.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Talabat.Core.Specifications.DoctorSpecs;
using Azure.Core;

namespace Talabat.API.Controllers
{
    public class AppointmentsController : BaseApiController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsController(IAppointmentService appointmentService, IUnitOfWork unitOfWork)
        {
            _appointmentService = appointmentService;
            _unitOfWork = unitOfWork;
        }


        [HttpGet("available-slots/{doctorId:int}")]
        public async Task<IActionResult> GetAvailableSlots(int doctorId)
        {
            try
            {
                // Step 1: Retrieve doctor and check existence
                var dSpec = new DoctorWithWorkScheduleSpecifications(doctorId);
                var doctor = await _unitOfWork.Repository<Doctor>().GetWithSpecAsync(dSpec);
                if (doctor == null)
                {
                    return NotFound(new ApiResponse(404, "Doctor not found"));
                }

                // Step 2: Retrieve doctor’s latest policy (if SlotDuration is part of policy)
                // All of the upcoming code should be a common service
                var dpSpec = new DoctorPolicySpecifications(doctorId);
                var doctorPolicies = await _unitOfWork.Repository<DoctorPolicy>().GetAllWithSpecAsync(dpSpec);

                // If no custom policies exist, return the default policy
                if (!doctorPolicies.Any())
                {
                    var dpfSpec = new DefaultDoctorPolicySpecifications();
                    doctorPolicies = await _unitOfWork.Repository<DoctorPolicy>().GetAllWithSpecAsync(dpfSpec);

                    if (doctorPolicies == null || !doctorPolicies.Any())
                    {
                        // Handle missing policy gracefully
                        return NotFound(new ApiResponse(404, "Default Doctor's policy not found"));
                    }
                }
                // This is either the default or the latest custom
                var latestPolicy = doctorPolicies.OrderByDescending(p => p.CreatedAt).First();
                // End of service

                // Step 4: Get booked appointments for the selected week
                var result = await _appointmentService.GetAvailableSlotsAsync(doctor);

                if (!result.IsSuccess)
                    return NotFound(new ApiResponse(404, result.ErrorMessage));

                return Ok(result.Data);
            }

            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, $"Error: {ex.Message}"));
            }
        }


        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto request)
        {
            // 1️⃣ Validate Doctor Exists
            var doctor = await _unitOfWork.Repository<Doctor>().GetAsync(request.doctorId);
            if (doctor == null)
                return NotFound(new ApiResponse(404, "Doctor not found"));

            // 2️⃣ Validate Patient Exists
            var patient = await _unitOfWork.Repository<Patient>().GetAsync(request.patientId);
            if (patient == null)
                return NotFound(new ApiResponse(404, "Patient not found"));

            // All of the upcoming code should be a common service
            var dpSpec = new DoctorPolicySpecifications(request.doctorId);
            var doctorPolicies = await _unitOfWork.Repository<DoctorPolicy>().GetAllWithSpecAsync(dpSpec);

            // If no custom policies exist, return the default policy
            if (!doctorPolicies.Any())
            {
                var dpfSpec = new DefaultDoctorPolicySpecifications();
                doctorPolicies = await _unitOfWork.Repository<DoctorPolicy>().GetAllWithSpecAsync(dpfSpec);

                if (doctorPolicies == null || !doctorPolicies.Any())
                {
                    // Handle missing policy gracefully
                    return NotFound(new ApiResponse(404, "Default Doctor's policy not found"));
                }
            }
            // This is either the default or the latest custom
            var latestPolicy = doctorPolicies.OrderByDescending(p => p.CreatedAt).First();
            // End of service

            // 3️⃣ Check if the doctor is available on the given day
            var wsSpec = new WorkShedulewithDoctorAndDayCriteriaSpecifications(request.doctorId, request.date.DayOfWeek);
            var workSchedules = await _unitOfWork.Repository<WorkSchedule>().GetAllWithSpecAsync(wsSpec);
            if (!workSchedules.Any())
                return NotFound(new ApiResponse(404, "Doctor doesn't work that day")); // Doctor doesn't work that day

            // 4️⃣ Check if the selected time falls within any shift
            bool isTimeValid = workSchedules.Any(ws => request.time >= ws.StartTime && request.time < ws.EndTime);
            if (!isTimeValid)
                return NotFound(new ApiResponse(404, "Time is outside of work schedule")); // Time is outside of work schedule

            // Step 5: Check if the appointment is in the generated slots
            var availableSlotsResult = await _appointmentService.GetAvailableSlotsAsync(doctor);

            // Check if the result was successful.
            if (!availableSlotsResult.IsSuccess)
            {
                return NotFound(new ApiResponse(404, availableSlotsResult.ErrorMessage));
            }

            // Check if the doctor has work schedules (to ensure slots are generated).
            if (!availableSlotsResult.Data.Any())
            {
                return NotFound(new ApiResponse(404, "No available slots for the doctor To Book."));
            }

            // Step 2: Check if the selected time is available
            var availableSlots = availableSlotsResult.Data.Values.Any(daySlots => daySlots.Contains(request.time));

            if (!availableSlots)
            {
                return BadRequest(new ApiResponse(400, "The appointment time is not available."));
            }

            // Apply policy checks such as prepayment, cancellation window, etc.
            if (latestPolicy.RequirePrePayment)
            {
                // Check if payment is done; if not, return an error
                // Assuming you have a payment check method or some payment flag on the patient
                //if (!patient.HasPaid)
                //    return BadRequest(new ApiResponse(400, "Payment required before confirming appointment"));
            }

            // Step 7: Create and Save Appointment
            var appointment = new Appointment
            {
                DoctorId = request.doctorId,
                PatientId = request.patientId,
                AppointmentDate = request.date,
                AppointmentTime = request.time,
                RescheduleCount = 0,
                Status = AppointmentStatus.Confirmed, // Assuming the doctor approves first
                PolicyId = latestPolicy.Id
            };

            await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
            var result = await _unitOfWork.CompleteAsync();
            if (result == 0)
            {
                return BadRequest(new ApiResponse(400));
            }

            return Ok(new ApiResponse(200, "Appointment booked successfully!"));
        }


        [HttpDelete("cancel-booking/{appointmentId:int}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
                // Step 1: Get the appointment and related entities
                var appointment = await _unitOfWork.Repository<Appointment>().GetAsync(appointmentId);
                if (appointment == null)
                {
                    return NotFound(new ApiResponse(404, "Appointment not found"));
                }

                // All of the upcoming code should be a common service
                var dpSpec = new DoctorPolicySpecifications(appointment.DoctorId);
                var doctorPolicies = await _unitOfWork.Repository<DoctorPolicy>().GetAllWithSpecAsync(dpSpec);

                // If no custom policies exist, return the default policy
                if (!doctorPolicies.Any())
                {
                    var dpfSpec = new DefaultDoctorPolicySpecifications();
                    doctorPolicies = await _unitOfWork.Repository<DoctorPolicy>().GetAllWithSpecAsync(dpfSpec);

                    if (doctorPolicies == null || !doctorPolicies.Any())
                    {
                        // Handle missing policy gracefully
                        return NotFound(new ApiResponse(404, "Default Doctor's policy not found"));
                    }
                }
                // This is either the default or the latest custom
                var latestPolicy = doctorPolicies.OrderByDescending(p => p.CreatedAt).First();
                // End of service

                // Step 2: Check if cancellation is allowed based on the policy and the time difference
                var currentTime = DateTime.UtcNow;
                var appointmentTime = appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime);

                var hoursBeforeAppointment = (appointmentTime - currentTime).TotalHours;

                if (hoursBeforeAppointment < latestPolicy.MinCancellationHours)
                {
                    // If the cancellation is too late
                    if (latestPolicy.AllowLateCancellationReschedule)
                    {
                        // Option to reschedule (if allowed in the policy)
                        return Ok(new ApiResponse(200, "Late cancellation allowed. You may reschedule."));
                    }
                    else
                    {
                        // No cancellation allowed, refund, or reschedule options
                        return BadRequest(new ApiResponse(400, "Late cancellations are not allowed."));
                    }
                }

                //// Step 3: Refund logic based on cancellation time
                //bool isRefundEligible = latestPolicy.AllowFullRefund;
                //decimal refundAmount = isRefundEligible ? appointment.AmountPaid : 0; // Adjust based on policy

                //// Step 4: Process refund if eligible and update the appointment status
                //if (refundAmount > 0)
                //{
                //    // Handle refund logic
                //    // E.g., calling a payment service to refund the amount
                //}

                // Step 5: Mark the appointment as canceled
                appointment.Status = AppointmentStatus.Cancelled;
                //appointment.IsCanceled = true; // Track cancellation state
                _unitOfWork.Repository<Appointment>().Update(appointment);

                var result = await _unitOfWork.CompleteAsync();
                if (result == 0)
                {
                    return BadRequest(new ApiResponse(400, "Error canceling the appointment"));
                }

                return Ok(new ApiResponse(200, "Appointment successfully canceled and refunded."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, $"Error: {ex.Message}"));
            }
        }



    }
}
