using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class MakeOriginIdUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Telemeds",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Streets",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Stations",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Specialities",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Doctors",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Districs",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Clinics",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Branches",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Telemeds_OriginId",
                table: "Telemeds",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_OriginId",
                table: "Streets",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_OriginId",
                table: "Stations",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_OriginId",
                table: "Specialities",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_OriginId",
                table: "Doctors",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districs_OriginId",
                table: "Districs",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_OriginId",
                table: "Clinics",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_OriginId",
                table: "Cities",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_OriginId",
                table: "Branches",
                column: "OriginId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Telemeds_OriginId",
                table: "Telemeds");

            migrationBuilder.DropIndex(
                name: "IX_Streets_OriginId",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_Stations_OriginId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_OriginId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_OriginId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Districs_OriginId",
                table: "Districs");

            migrationBuilder.DropIndex(
                name: "IX_Clinics_OriginId",
                table: "Clinics");

            migrationBuilder.DropIndex(
                name: "IX_Cities_OriginId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Branches_OriginId",
                table: "Branches");

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Telemeds",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Streets",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Stations",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Specialities",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Districs",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Clinics",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Branches",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}
