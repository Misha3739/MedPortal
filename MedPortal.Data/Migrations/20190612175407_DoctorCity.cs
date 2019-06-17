using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class DoctorCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Doctors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_CityId",
                table: "Doctors",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Cities_CityId",
                table: "Doctors",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Cities_CityId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_CityId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Doctors");
        }
    }
}
