using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class dsedew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaidFromBank",
                table: "Cases",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PaidFromCompany",
                table: "Cases",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidFromBank",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "PaidFromCompany",
                table: "Cases");
        }
    }
}
