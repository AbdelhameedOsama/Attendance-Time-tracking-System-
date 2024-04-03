using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Time_Tracking.Migrations
{
    /// <inheritdoc />
    public partial class fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Tracks_TrackID",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Intakes_Programs_ProgramId",
                table: "Intakes");

            migrationBuilder.DropForeignKey(
                name: "FK_Intakes_Programs_ProgramsID",
                table: "Intakes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Instructors_SupId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Intakes_ProgramsID",
                table: "Intakes");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_TrackID",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "ProgramsID",
                table: "Intakes");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Instructors");

            migrationBuilder.RenameColumn(
                name: "SupId",
                table: "Tracks",
                newName: "SupID");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_SupId",
                table: "Tracks",
                newName: "IX_Tracks_SupID");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Intakes",
                newName: "ProgramID");

            migrationBuilder.RenameIndex(
                name: "IX_Intakes_ProgramId",
                table: "Intakes",
                newName: "IX_Intakes_ProgramID");

            migrationBuilder.AddForeignKey(
                name: "FK_Intakes_Programs_ProgramID",
                table: "Intakes",
                column: "ProgramID",
                principalTable: "Programs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Instructors_SupID",
                table: "Tracks",
                column: "SupID",
                principalTable: "Instructors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intakes_Programs_ProgramID",
                table: "Intakes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Instructors_SupID",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "SupID",
                table: "Tracks",
                newName: "SupId");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_SupID",
                table: "Tracks",
                newName: "IX_Tracks_SupId");

            migrationBuilder.RenameColumn(
                name: "ProgramID",
                table: "Intakes",
                newName: "ProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Intakes_ProgramID",
                table: "Intakes",
                newName: "IX_Intakes_ProgramId");

            migrationBuilder.AddColumn<int>(
                name: "ProgramsID",
                table: "Intakes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrackID",
                table: "Instructors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Intakes_ProgramsID",
                table: "Intakes",
                column: "ProgramsID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_TrackID",
                table: "Instructors",
                column: "TrackID");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Tracks_TrackID",
                table: "Instructors",
                column: "TrackID",
                principalTable: "Tracks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Intakes_Programs_ProgramId",
                table: "Intakes",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Intakes_Programs_ProgramsID",
                table: "Intakes",
                column: "ProgramsID",
                principalTable: "Programs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Instructors_SupId",
                table: "Tracks",
                column: "SupId",
                principalTable: "Instructors",
                principalColumn: "ID");
        }
    }
}
