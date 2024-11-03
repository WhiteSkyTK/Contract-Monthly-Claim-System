using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class AddedReportMeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportMetadata",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateGenerated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimID = table.Column<int>(type: "int", nullable: true),
                    LecturerID = table.Column<int>(type: "int", nullable: true),
                    ApprovalID = table.Column<int>(type: "int", nullable: true),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportMetadata", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReportMetadata_ApprovalProcesses_ApprovalID",
                        column: x => x.ApprovalID,
                        principalTable: "ApprovalProcesses",
                        principalColumn: "ApprovalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportMetadata_Claims_ClaimID",
                        column: x => x.ClaimID,
                        principalTable: "Claims",
                        principalColumn: "ClaimID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportMetadata_Lecturers_LecturerID",
                        column: x => x.LecturerID,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportMetadata_ApprovalID",
                table: "ReportMetadata",
                column: "ApprovalID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportMetadata_ClaimID",
                table: "ReportMetadata",
                column: "ClaimID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportMetadata_LecturerID",
                table: "ReportMetadata",
                column: "LecturerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportMetadata");
        }
    }
}
