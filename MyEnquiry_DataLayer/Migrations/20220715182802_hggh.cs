using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class hggh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                           name: "BankId",
                           table: "SurveyForms",
                           type: "int",
                           nullable: true,
                           defaultValue: 0);

       /*     migrationBuilder.AddColumn<int>(
                name: "CaseTypeId",
                table: "SurveyForms",
                type: "int",
                nullable: true,
                defaultValue: 0);*/
/*
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "SurveyFormResponses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyForms_BankId",
                table: "SurveyForms",
                column: "BankId");*/

            migrationBuilder.CreateIndex(
                name: "IX_SurveyForms_CaseTypeId",
                table: "SurveyForms",

                column: "CaseTypeId");

    /*        migrationBuilder.CreateIndex(
                name: "IX_SurveyFormResponses_CompanyId",
                table: "SurveyFormResponses",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyFormResponses_Companies_CompanyId",
                table: "SurveyFormResponses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyForms_Banks_BankId",
                table: "SurveyForms",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyForms_CaseTypes_CaseTypeId",
                table: "SurveyForms",
                column: "CaseTypeId",
                principalTable: "CaseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyFormResponses_Companies_CompanyId",
                table: "SurveyFormResponses");*/

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyForms_Banks_BankId",
                table: "SurveyForms");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyForms_CaseTypes_CaseTypeId",
                table: "SurveyForms");

            migrationBuilder.DropIndex(
                name: "IX_SurveyForms_BankId",
                table: "SurveyForms");

            migrationBuilder.DropIndex(
                name: "IX_SurveyForms_CaseTypeId",
                table: "SurveyForms");

            migrationBuilder.DropIndex(
                name: "IX_SurveyFormResponses_CompanyId",
                table: "SurveyFormResponses");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "SurveyForms");

            migrationBuilder.DropColumn(
                name: "CaseTypeId",
                table: "SurveyForms");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "SurveyFormResponses");
        }

    
    }
}
