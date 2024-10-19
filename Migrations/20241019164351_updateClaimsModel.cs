using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class updateClaimsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalProcesses_AcademicManagers_ManagerID",
                table: "ApprovalProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalProcesses_Claims_ClaimID",
                table: "ApprovalProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalProcesses_ProgrammeCoordinators_CoordinatorID",
                table: "ApprovalProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AcademicManagers_AcademicManagerManagerID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Modules_ModuleCode",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_ProgrammeCoordinatorCoordinatorID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_AcademicManagerManagerID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ModuleCode",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ProgrammeCoordinatorCoordinatorID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "AcademicManagerManagerID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ModuleCode",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ProgrammeCoordinatorCoordinatorID",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "ApprovalProcesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalProcesses_AcademicManagers_ManagerID",
                table: "ApprovalProcesses",
                column: "ManagerID",
                principalTable: "AcademicManagers",
                principalColumn: "ManagerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalProcesses_Claims_ClaimID",
                table: "ApprovalProcesses",
                column: "ClaimID",
                principalTable: "Claims",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalProcesses_ProgrammeCoordinators_CoordinatorID",
                table: "ApprovalProcesses",
                column: "CoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalProcesses_AcademicManagers_ManagerID",
                table: "ApprovalProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalProcesses_Claims_ClaimID",
                table: "ApprovalProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalProcesses_ProgrammeCoordinators_CoordinatorID",
                table: "ApprovalProcesses");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "ApprovalProcesses");

            migrationBuilder.AddColumn<int>(
                name: "AcademicManagerManagerID",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleCode",
                table: "Claims",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
                name: "IX_Claims_ModuleCode",
                table: "Claims",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ProgrammeCoordinatorCoordinatorID",
                table: "Claims",
                column: "ProgrammeCoordinatorCoordinatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalProcesses_AcademicManagers_ManagerID",
                table: "ApprovalProcesses",
                column: "ManagerID",
                principalTable: "AcademicManagers",
                principalColumn: "ManagerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalProcesses_Claims_ClaimID",
                table: "ApprovalProcesses",
                column: "ClaimID",
                principalTable: "Claims",
                principalColumn: "ClaimID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalProcesses_ProgrammeCoordinators_CoordinatorID",
                table: "ApprovalProcesses",
                column: "CoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AcademicManagers_AcademicManagerManagerID",
                table: "Claims",
                column: "AcademicManagerManagerID",
                principalTable: "AcademicManagers",
                principalColumn: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Modules_ModuleCode",
                table: "Claims",
                column: "ModuleCode",
                principalTable: "Modules",
                principalColumn: "ModuleCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ProgrammeCoordinators_ProgrammeCoordinatorCoordinatorID",
                table: "Claims",
                column: "ProgrammeCoordinatorCoordinatorID",
                principalTable: "ProgrammeCoordinators",
                principalColumn: "CoordinatorID");
        }
    }
}
