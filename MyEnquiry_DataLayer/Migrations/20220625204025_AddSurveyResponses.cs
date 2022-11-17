using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class AddSurveyResponses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyFormResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyFormId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyFormElementId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFile = table.Column<bool>(type: "bit", nullable: false),
                    FileUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyFormResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyFormResponses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyFormResponses_SurveyFormElements_SurveyFormElementId",
                        column: x => x.SurveyFormElementId,
                        principalTable: "SurveyFormElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SurveyFormResponses_SurveyForms_SurveyFormId",
                        column: x => x.SurveyFormId,
                        principalTable: "SurveyForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFormResponses_SurveyFormElementId",
                table: "SurveyFormResponses",
                column: "SurveyFormElementId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFormResponses_SurveyFormId",
                table: "SurveyFormResponses",
                column: "SurveyFormId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFormResponses_UserId",
                table: "SurveyFormResponses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyFormResponses");
        }
    }
}
