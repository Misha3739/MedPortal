using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class FixHDoctors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Telemeds_TelemedId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_TelemedId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "TelemedId",
                table: "Doctors");

            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "Doctors",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "Doctors",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TelemedId",
                table: "Doctors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_TelemedId",
                table: "Doctors",
                column: "TelemedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Telemeds_TelemedId",
                table: "Doctors",
                column: "TelemedId",
                principalTable: "Telemeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
