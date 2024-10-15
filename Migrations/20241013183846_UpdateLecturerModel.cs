using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLecturerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_CoordintorID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_CoordintorID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "CoordintorID",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "CoordintorID",
                table: "ProgrammeCoordinators",
                newName: "CoordinatorID");

            migrationBuilder.AddColumn<string>(
                name: "CoordintorSurname",
                table: "ProgrammeCoordinators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LecturerPhone",
                table: "Lecturers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerSurname",
                table: "AcademicManagers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AcademicManagers",
                columns: new[] { "ManagerID", "ManagerEmail", "ManagerName", "ManagerSurname" },
                values: new object[] { 1, "Bobrown@gmail.com", "Bob", "Brown" });

            migrationBuilder.InsertData(
                table: "Lecturers",
                columns: new[] { "LecturerID", "HourlyRate", "LecturerEmail", "LecturerName", "LecturerPhone", "LecturerSurname" },
                values: new object[] { 1, 0m, "john.doe@example.com", "John", "083-123-4567", "Doe" });

            migrationBuilder.InsertData(
                table: "ProgrammeCoordinators",
                columns: new[] { "CoordinatorID", "CoordintorEmail", "CoordintorName", "CoordintorSurname" },
                values: new object[] { 1, "Alicesmith@gmail.com", "Alice", "Smith" });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "ClaimID", "AdditionalNotes", "CoordinatorID", "HourlyRate", "HoursWorked", "LecturerID", "ManagerID", "Module", "Status", "SubmissionDate" },
                values: new object[] { 1, "Worked on project.", 1, 100m, 10, 1, 0, "PROG6212", "Pending", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ApprovalProcesses",
                columns: new[] { "ApprovalID", "ApprovalDate", "ApprovalStatus", "ClaimID", "CoordinatorID", "ManagerID" },
                values: new object[] { 1, new DateTime(2024, 10, 13, 20, 38, 45, 592, DateTimeKind.Local).AddTicks(8352), "Pending", 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CoordinatorID",
                table: "Claims",
                column: "CoordinatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_CoordinatorID",
                table: "Claims",
                column: "CoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_CoordinatorID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_CoordinatorID",
                table: "Claims");

            migrationBuilder.DeleteData(
                table: "ApprovalProcesses",
                keyColumn: "ApprovalID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AcademicManagers",
                keyColumn: "ManagerID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "ClaimID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProgrammeCoordinators",
                keyColumn: "CoordinatorID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CoordintorSurname",
                table: "ProgrammeCoordinators");

            migrationBuilder.DropColumn(
                name: "LecturerPhone",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ManagerSurname",
                table: "AcademicManagers");

            migrationBuilder.RenameColumn(
                name: "CoordinatorID",
                table: "ProgrammeCoordinators",
                newName: "CoordintorID");

            migrationBuilder.AddColumn<int>(
                name: "CoordintorID",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CoordintorID",
                table: "Claims",
                column: "CoordintorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_CoordintorID",
                table: "Claims",
                column: "CoordintorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordintorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
