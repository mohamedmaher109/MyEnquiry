using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class eww : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CasesId",
                table: "Cases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Cases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CasesId",
                table: "Cases",
                column: "CasesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Cases_CasesId",
                table: "Cases",
                column: "CasesId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Cases_CasesId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CasesId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CasesId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Cases");
        }
    }
}
