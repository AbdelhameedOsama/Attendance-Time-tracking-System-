using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Time_Tracking.Migrations
{
    /// <inheritdoc />
    public partial class ISSupervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupervisor",
                table: "Instructors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupervisor",
                table: "Instructors");
        }
    }
}
