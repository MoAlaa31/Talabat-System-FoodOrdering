using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotDurationMinutes",
                table: "DoctorPolicies");

            migrationBuilder.AddColumn<int>(
                name: "SlotDurationMinutes",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotDurationMinutes",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "SlotDurationMinutes",
                table: "DoctorPolicies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DoctorPolicies",
                keyColumn: "Id",
                keyValue: 1,
                column: "SlotDurationMinutes",
                value: 20);
        }
    }
}
