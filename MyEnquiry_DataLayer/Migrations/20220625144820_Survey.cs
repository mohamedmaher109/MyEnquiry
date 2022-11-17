using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class Survey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAcceptingResponses = table.Column<bool>(type: "bit", nullable: false),
                    Logo1Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo1Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo2Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo2Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyFormElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyFormId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCheckbox = table.Column<bool>(type: "bit", nullable: false),
                    IsTextbox = table.Column<bool>(type: "bit", nullable: false),
                    IsTextarea = table.Column<bool>(type: "bit", nullable: false),
                    IsRadioButton = table.Column<bool>(type: "bit", nullable: false),
                    IsSelect = table.Column<bool>(type: "bit", nullable: false),
                    IsDate = table.Column<bool>(type: "bit", nullable: false),
                    IsFile = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CheckBoxText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTextboxEmail = table.Column<bool>(type: "bit", nullable: false),
                    IsTextboxPassword = table.Column<bool>(type: "bit", nullable: false),
                    IsTextboxNumber = table.Column<bool>(type: "bit", nullable: false),
                    TextboxMinLength = table.Column<int>(type: "int", nullable: true),
                    TextboxMaxLength = table.Column<int>(type: "int", nullable: true),
                    TextareaMinLength = table.Column<int>(type: "int", nullable: true),
                    TextareaMaxLength = table.Column<int>(type: "int", nullable: true),
                    FileAcceptedFileExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNumberOfMinimumFilesAllowed = table.Column<int>(type: "int", nullable: true),
                    FileNumberOfMaximumFilesAllowed = table.Column<int>(type: "int", nullable: true),
                    SelectBoxOptionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyFormElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyFormElements_SurveyForms_SurveyFormId",
                        column: x => x.SurveyFormId,
                        principalTable: "SurveyForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyFormElementItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyFormElementId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyFormElementItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyFormElementItems_SurveyFormElements_SurveyFormElementId",
                        column: x => x.SurveyFormElementId,
                        principalTable: "SurveyFormElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFormElementItems_SurveyFormElementId",
                table: "SurveyFormElementItems",
                column: "SurveyFormElementId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyFormElements_SurveyFormId",
                table: "SurveyFormElements",
                column: "SurveyFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyFormElementItems");

            migrationBuilder.DropTable(
                name: "SurveyFormElements");

            migrationBuilder.DropTable(
                name: "SurveyForms");
        }
    }
}
