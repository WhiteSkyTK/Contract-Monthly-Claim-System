using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class InitailCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicManagers",
                columns: table => new
                {
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicManagers", x => x.ManagerID);
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    LecturerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LecturerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.LecturerID);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleCode);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammeCoordinators",
                columns: table => new
                {
                    CoordinatorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoordinatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammeCoordinators", x => x.CoordinatorID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "LecturerModules",
                columns: table => new
                {
                    LecturerID = table.Column<int>(type: "int", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LecturerModulesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerModules", x => new { x.LecturerID, x.ModuleCode });
                    table.ForeignKey(
                        name: "FK_LecturerModules_Lecturers_LecturerID",
                        column: x => x.LecturerID,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LecturerModules_Modules_ModuleCode",
                        column: x => x.ModuleCode,
                        principalTable: "Modules",
                        principalColumn: "ModuleCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoursWorked = table.Column<int>(type: "int", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LecturerID = table.Column<int>(type: "int", nullable: false),
                    CoordinatorID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false),
                    TotalClaimAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimID);
                    table.ForeignKey(
                        name: "FK_Claims_AcademicManagers_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "AcademicManagers",
                        principalColumn: "ManagerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Lecturers_LecturerID",
                        column: x => x.LecturerID,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Claims_Modules_ModuleCode",
                        column: x => x.ModuleCode,
                        principalTable: "Modules",
                        principalColumn: "ModuleCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claims_ProgrammeCoordinators_CoordinatorID",
                        column: x => x.CoordinatorID,
                        principalTable: "ProgrammeCoordinators",
                        principalColumn: "CoordinatorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalProcesses",
                columns: table => new
                {
                    ApprovalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimID = table.Column<int>(type: "int", nullable: false),
                    CoordinatorID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalProcesses", x => x.ApprovalID);
                    table.ForeignKey(
                        name: "FK_ApprovalProcesses_AcademicManagers_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "AcademicManagers",
                        principalColumn: "ManagerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalProcesses_Claims_ClaimID",
                        column: x => x.ClaimID,
                        principalTable: "Claims",
                        principalColumn: "ClaimID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalProcesses_ProgrammeCoordinators_CoordinatorID",
                        column: x => x.CoordinatorID,
                        principalTable: "ProgrammeCoordinators",
                        principalColumn: "CoordinatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimsModules",
                columns: table => new
                {
                    ClaimID = table.Column<int>(type: "int", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimsModulesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimsModules", x => new { x.ClaimID, x.ModuleCode });
                    table.ForeignKey(
                        name: "FK_ClaimsModules_Claims_ClaimID",
                        column: x => x.ClaimID,
                        principalTable: "Claims",
                        principalColumn: "ClaimID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimsModules_Modules_ModuleCode",
                        column: x => x.ModuleCode,
                        principalTable: "Modules",
                        principalColumn: "ModuleCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportingDocuments",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportingDocuments", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_SupportingDocuments_Claims_ClaimID",
                        column: x => x.ClaimID,
                        principalTable: "Claims",
                        principalColumn: "ClaimID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalProcesses_ClaimID",
                table: "ApprovalProcesses",
                column: "ClaimID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalProcesses_CoordinatorID",
                table: "ApprovalProcesses",
                column: "CoordinatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalProcesses_ManagerID",
                table: "ApprovalProcesses",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CoordinatorID",
                table: "Claims",
                column: "CoordinatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_LecturerID",
                table: "Claims",
                column: "LecturerID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ManagerID",
                table: "Claims",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ModuleCode",
                table: "Claims",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimsModules_ModuleCode",
                table: "ClaimsModules",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerModules_ModuleCode",
                table: "LecturerModules",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_SupportingDocuments_ClaimID",
                table: "SupportingDocuments",
                column: "ClaimID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalProcesses");

            migrationBuilder.DropTable(
                name: "ClaimsModules");

            migrationBuilder.DropTable(
                name: "LecturerModules");

            migrationBuilder.DropTable(
                name: "SupportingDocuments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "AcademicManagers");

            migrationBuilder.DropTable(
                name: "Lecturers");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "ProgrammeCoordinators");
        }
    }
}
