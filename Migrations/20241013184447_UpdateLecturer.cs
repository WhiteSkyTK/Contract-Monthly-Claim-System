using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLecturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApprovalProcesses",
                keyColumn: "ApprovalID",
                keyValue: 1,
                column: "ApprovalDate",
                value: new DateTime(2024, 10, 13, 20, 44, 47, 165, DateTimeKind.Local).AddTicks(86));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApprovalProcesses",
                keyColumn: "ApprovalID",
                keyValue: 1,
                column: "ApprovalDate",
                value: new DateTime(2024, 10, 13, 20, 38, 45, 592, DateTimeKind.Local).AddTicks(8352));
        }
    }
}
