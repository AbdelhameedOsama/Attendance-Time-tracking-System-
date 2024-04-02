using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Time_Tracking.Migrations
{
    /// <inheritdoc />
    public partial class imstructorUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "studentId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "instructorId",
                table: "Instructors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Attendance",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_studentId",
                table: "Students",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_instructorId",
                table: "Instructors",
                column: "instructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_InstructorId",
                table: "Attendance",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Instructors_InstructorId",
                table: "Attendance",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Instructors_InstructorId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Students_studentId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_instructorId",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_InstructorId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "studentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "instructorId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Attendance");
        }
    }
}
