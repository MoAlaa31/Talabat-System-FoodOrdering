using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Calender
{
    public class DoctorPolicy: BaseEntity
    {
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public bool IsDefault { get; set; }  // 🔹 Indicates if this is the default policy

        // ⏳ Cancellation Rules
        public bool AllowPatientCancellation { get; set; } = true; // Can the patient cancel at all?
        public int MinCancellationHours { get; set; } = 24; // Hours before an appointment to cancel without penalty
        public bool AllowLateCancellationReschedule { get; set; } = true; // Can patient reschedule instead of losing payment?
        public int MaxRescheduleAttempts { get; set; } = 1; // How many times a patient can reschedule an appointment?

        // 📅 Rescheduling Rules
        public bool AllowRescheduling { get; set; } = true; // Can the patient reschedule at all?
        public int MinRescheduleHours { get; set; } = 12; // Hours before an appointment to reschedule

        // 💰 Refund & Payment Rules
        public bool AllowFullRefund { get; set; } = true; // If patient cancels early, do they get a full refund?
        public bool AllowPartialRefund { get; set; } = true; // If patient cancels late, do they get a partial refund?
        public decimal PartialRefundPercentage { get; set; } = 50; // Percentage of money refunded if canceled late
        public bool RequirePrePayment { get; set; } = true; // Does the patient need to pay before confirming the booking?
        public int UnpaidReservationTimeoutMinutes { get; set; } = 30; // Auto-cancel unpaid bookings after X minutes

        // 🏥 Doctor Availability & Scheduling
        public bool AllowMultipleBookingsPerDay { get; set; } = false; // Can a patient book more than one appointment per day?
        public int MaxBookingsPerPatientPerDay { get; set; } = 1; // If allowed, how many times can a patient book per day?
        public bool AllowLastMinuteBooking { get; set; } = true; // Can patients book minutes before the appointment?
        public int MinBookingAdvanceHours { get; set; } = 2; // Minimum hours before appointment to allow booking


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Track when this policy was created

        //// 🔄 Doctor Schedule Updates & Patient Impact
        //public bool AutoRescheduleIfDoctorChangesSchedule { get; set; } = true; // If doctor updates schedule, should system auto-reschedule affected patients?
        //public bool AllowDoctorToCancelAnytime { get; set; } = true; // Can doctor cancel an appointment anytime without penalty?
        //public bool AllowDoctorToBlockDays { get; set; } = true; // Can doctor block entire days due to personal reasons?
    }

}
