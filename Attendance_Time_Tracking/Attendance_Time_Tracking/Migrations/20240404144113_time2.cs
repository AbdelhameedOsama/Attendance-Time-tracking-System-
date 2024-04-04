using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Time_Tracking.Migrations
{
    /// <inheritdoc />
    public partial class time2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Arrival_Time = table.Column<TimeOnly>(type: "datetime2", nullable: false),
                    Departure_Time = table.Column<TimeOnly>(type: "datetime2", nullable: true)
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
