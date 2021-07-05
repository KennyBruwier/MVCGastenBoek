using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCGastenBoek.Migrations
{
    public partial class AddStraat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Straat",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Straat",
                table: "AspNetUsers");
        }
    }
}
