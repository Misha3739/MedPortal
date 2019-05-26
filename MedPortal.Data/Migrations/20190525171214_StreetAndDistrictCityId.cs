using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class StreetAndDistrictCityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Streets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Districs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_CityId",
                table: "Streets",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Districs_CityId",
                table: "Districs",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districs_Cities_CityId",
                table: "Districs",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Streets_Cities_CityId",
                table: "Streets",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districs_Cities_CityId",
                table: "Districs");

            migrationBuilder.DropForeignKey(
                name: "FK_Streets_Cities_CityId",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_Streets_CityId",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_Districs_CityId",
                table: "Districs");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Districs");
        }
    }
}
