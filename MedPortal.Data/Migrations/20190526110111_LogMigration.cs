using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class LogMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncomeTime = table.Column<DateTime>(nullable: false),
                    OutcomeTime = table.Column<DateTime>(nullable: false),
                    RequestedUrl = table.Column<string>(maxLength: 200, nullable: true),
                    Ip = table.Column<string>(maxLength: 50, nullable: true),
                    StatusCode = table.Column<int>(nullable: false),
                    RequestBody = table.Column<string>(nullable: true),
                    ExceptionInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
