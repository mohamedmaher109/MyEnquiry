using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class dsadsdw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewerName",
                table: "Cases");

            migrationBuilder.AddColumn<bool>(
                name: "IsReiew",
                table: "Cases",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReiew",
                table: "Cases");

            migrationBuilder.AddColumn<string>(
                name: "ReviewerName",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
