using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class OriginIdFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Telemeds",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Streets",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Stations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Specialities",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Doctors",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Districs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Clinics",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Cities",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "Branches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Telemeds");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Districs");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Branches");
        }
    }
}
