using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class miosds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reviewr",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reviewr",
                table: "AspNetUsers");
        }
    }
}
