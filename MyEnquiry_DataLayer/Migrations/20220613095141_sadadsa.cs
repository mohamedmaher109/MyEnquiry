using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class sadadsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "Regions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CitiesId",
                table: "Regions",
                column: "CitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CitiesId",
                table: "AspNetUsers",
                column: "CitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_CitiesId",
                table: "AspNetUsers",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Cities_CitiesId",
                table: "Regions",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CitiesId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Cities_CitiesId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_CitiesId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CitiesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitiesId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "CitiesId",
                table: "AspNetUsers");
        }
    }
}
