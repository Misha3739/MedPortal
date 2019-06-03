using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class ClinicStationsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClinicStations_ClinicId",
                table: "ClinicStations");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicStations_ClinicId_StationId",
                table: "ClinicStations",
                columns: new[] { "ClinicId", "StationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClinicStations_ClinicId_StationId",
                table: "ClinicStations");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicStations_ClinicId",
                table: "ClinicStations",
                column: "ClinicId");
        }
    }
}
