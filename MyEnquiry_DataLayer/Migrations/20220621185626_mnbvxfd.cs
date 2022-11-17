using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class mnbvxfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "lat",
                table: "Cases",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "lng",
                table: "Cases",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lat",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Cases");
        }
    }
}
