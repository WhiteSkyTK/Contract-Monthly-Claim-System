using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class HRadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRs",
                columns: table => new
                {
                    HRID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HRSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HREmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HRPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HRPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRs", x => x.HRID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRs");
        }
    }
}
