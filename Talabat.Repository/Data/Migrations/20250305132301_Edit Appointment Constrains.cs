using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditAppointmentConstrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkSchedules_DoctorId_Day_StartTime",
                table: "WorkSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_DoctorId",
                table: "WorkSchedules",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkSchedules_DoctorId",
                table: "WorkSchedules");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_DoctorId_Day_StartTime",
                table: "WorkSchedules",
                columns: new[] { "DoctorId", "Day", "StartTime" },
                unique: true);
        }
    }
}
