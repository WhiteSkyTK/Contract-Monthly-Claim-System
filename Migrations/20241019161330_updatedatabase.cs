using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AcademicManagers_ManagerID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_CoordinatorID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_CoordinatorID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ManagerID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "CoordinatorID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ManagerID",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "AcademicManagerManagerID",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgrammeCoordinatorCoordinatorID",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_AcademicManagerManagerID",
                table: "Claims",
                column: "AcademicManagerManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ProgrammeCoordinatorCoordinatorID",
                table: "Claims",
                column: "ProgrammeCoordinatorCoordinatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AcademicManagers_AcademicManagerManagerID",
                table: "Claims",
                column: "AcademicManagerManagerID",
                principalTable: "AcademicManagers",
                principalColumn: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_ProgrammeCoordinatorCoordinatorID",
                table: "Claims",
                column: "ProgrammeCoordinatorCoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AcademicManagers_AcademicManagerManagerID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_ProgrammeCoordinatorCoordinatorID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_AcademicManagerManagerID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ProgrammeCoordinatorCoordinatorID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "AcademicManagerManagerID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ProgrammeCoordinatorCoordinatorID",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "CoordinatorID",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManagerID",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CoordinatorID",
                table: "Claims",
                column: "CoordinatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ManagerID",
                table: "Claims",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AcademicManagers_ManagerID",
                table: "Claims",
                column: "ManagerID",
                principalTable: "AcademicManagers",
                principalColumn: "ManagerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_CoordinatorID",
                table: "Claims",
                column: "CoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
