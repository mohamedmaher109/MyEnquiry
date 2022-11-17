using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEnquiry_DataLayer.Migrations
{
    public partial class bvcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CitiesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CitiesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitiesId",
                table: "AspNetUsers");*/

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "AspNetUsers",
                newName: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_cityId",
                table: "AspNetUsers",
                column: "cityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_cityId",
                table: "AspNetUsers",
                column: "cityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_cityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_cityId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "cityId",
                table: "AspNetUsers",
                newName: "CityId");

            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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
        }
    }
}
