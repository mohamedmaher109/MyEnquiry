using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class sdsda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCamira",
                table: "SurveyFormElements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCamira",
                table: "SurveyFormElements");
        }
    }
}
