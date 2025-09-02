using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondSeedPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DoctorPolicies",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DoctorPolicies",
                columns: new[] { "Id", "AllowFullRefund", "AllowLastMinuteBooking", "AllowLateCancellationReschedule", "AllowMultipleBookingsPerDay", "AllowPartialRefund", "AllowPatientCancellation", "AllowRescheduling", "CreatedAt", "DoctorId", "IsDefault", "MaxBookingsPerPatientPerDay", "MaxRescheduleAttempts", "MinBookingAdvanceHours", "MinCancellationHours", "MinRescheduleHours", "PartialRefundPercentage", "RequirePrePayment", "UnpaidReservationTimeoutMinutes" },
                values: new object[] { 1, true, true, true, false, true, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 1, 1, 2, 24, 12, 50m, true, 30 });
        }
    }
}
