using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class ClinicSpecialities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchAlias",
                table: "Specialities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "Specialities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSimpe",
                table: "Specialities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NameGenitive",
                table: "Specialities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NamePlural",
                table: "Specialities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NamePluralGenitive",
                table: "Specialities",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClinicSpecialities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClinicId = table.Column<long>(nullable: false),
                    SpecialityId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicSpecialities_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClinicSpecialities_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicSpecialities_ClinicId",
                table: "ClinicSpecialities",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicSpecialities_SpecialityId",
                table: "ClinicSpecialities",
                column: "SpecialityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicSpecialities");

            migrationBuilder.DropColumn(
                name: "BranchAlias",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "IsSimpe",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "NameGenitive",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "NamePlural",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "NamePluralGenitive",
                table: "Specialities");
        }
    }
}
