using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class DropClinicParentConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Clinics_ParentId",
                table: "Clinics");

            migrationBuilder.DropIndex(
                name: "IX_Clinics_ParentId",
                table: "Clinics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Clinics_ParentId",
                table: "Clinics",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Clinics_ParentId",
                table: "Clinics",
                column: "ParentId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
