using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class sdsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "SurveyFormResponses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFormResponses_CaseId",
                table: "SurveyFormResponses",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyFormResponses_Cases_CaseId",
                table: "SurveyFormResponses",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyFormResponses_Cases_CaseId",
                table: "SurveyFormResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyFormResponses_CaseId",
                table: "SurveyFormResponses");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "SurveyFormResponses");
        }
    }
}
