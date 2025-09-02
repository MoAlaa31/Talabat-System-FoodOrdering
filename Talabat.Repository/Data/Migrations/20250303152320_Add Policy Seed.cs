using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivePolicyId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "DoctorPolicies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "DoctorPolicies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "DoctorPolicies",
                columns: new[] { "Id", "AllowFullRefund", "AllowLastMinuteBooking", "AllowLateCancellationReschedule", "AllowMultipleBookingsPerDay", "AllowPartialRefund", "AllowPatientCancellation", "AllowRescheduling", "CreatedAt", "DoctorId", "IsDefault", "MaxBookingsPerPatientPerDay", "MaxRescheduleAttempts", "MinBookingAdvanceHours", "MinCancellationHours", "MinRescheduleHours", "PartialRefundPercentage", "RequirePrePayment", "SlotDurationMinutes", "UnpaidReservationTimeoutMinutes" },
                values: new object[] { 1, true, true, true, false, true, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 1, 1, 2, 24, 12, 50m, true, 20, 30 });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ActivePolicyId",
                table: "Doctors",
                column: "ActivePolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorPolicies_ActivePolicyId",
                table: "Doctors",
                column: "ActivePolicyId",
                principalTable: "DoctorPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorPolicies_ActivePolicyId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ActivePolicyId",
                table: "Doctors");

            migrationBuilder.DeleteData(
                table: "DoctorPolicies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "ActivePolicyId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "DoctorPolicies");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "DoctorPolicies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
