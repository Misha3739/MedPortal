using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class DropHBranchTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Branches_BranchId",
                table: "Stations");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Stations_BranchId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Stations");

            migrationBuilder.AddColumn<string>(
                name: "LineColor",
                table: "Stations",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LineName",
                table: "Stations",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineColor",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "LineName",
                table: "Stations");

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "Stations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<long>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    LineColor = table.Column<string>(maxLength: 6, nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    OriginId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_BranchId",
                table: "Stations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CityId",
                table: "Branches",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_OriginId",
                table: "Branches",
                column: "OriginId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Branches_BranchId",
                table: "Stations",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
