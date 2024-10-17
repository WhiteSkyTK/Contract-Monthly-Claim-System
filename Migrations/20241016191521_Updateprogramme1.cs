using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class Updateprogramme1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LecturerPassword",
                table: "ProgrammeCoordinators",
                newName: "CoordinatorPassword");

            migrationBuilder.RenameColumn(
                name: "LecturerPassword",
                table: "AcademicManagers",
                newName: "ManagerPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoordinatorPassword",
                table: "ProgrammeCoordinators",
                newName: "LecturerPassword");

            migrationBuilder.RenameColumn(
                name: "ManagerPassword",
                table: "AcademicManagers",
                newName: "LecturerPassword");
        }
    }
}
