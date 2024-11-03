using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class updatedClaimsmodule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimsModulesID",
                table: "ClaimsModules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimsModulesID",
                table: "ClaimsModules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
