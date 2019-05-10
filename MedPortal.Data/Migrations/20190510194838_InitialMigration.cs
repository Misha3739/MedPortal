using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPortal.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    LineColor = table.Column<string>(maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Alias = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    BranchId = table.Column<long>(nullable: false),
                    CityId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(maxLength: 200, nullable: true),
                    RewriteName = table.Column<string>(maxLength: 200, nullable: true),
                    Url = table.Column<string>(nullable: true),
                    HCityId = table.Column<long>(nullable: false),
                    HStreetId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    House = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    HDistrictId = table.Column<long>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    OnlineRecordDoctor = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clinics_Cities_HCityId",
                        column: x => x.HCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clinics_Districs_HDistrictId",
                        column: x => x.HDistrictId,
                        principalTable: "Districs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clinics_Streets_HStreetId",
                        column: x => x.HStreetId,
                        principalTable: "Streets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clinics_Clinics_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Telemeds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Chat = table.Column<bool>(nullable: false),
                    Phone = table.Column<bool>(nullable: false),
                    HClinicId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telemeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telemeds_Clinics_HClinicId",
                        column: x => x.HClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Rating = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    Img = table.Column<string>(nullable: true),
                    ImgFormat = table.Column<string>(nullable: true),
                    AddPhoneNumber = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Degree = table.Column<string>(nullable: true),
                    Rank = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TextEducation = table.Column<string>(nullable: true),
                    TextDegree = table.Column<string>(nullable: true),
                    TextSpec = table.Column<string>(nullable: true),
                    TextCourse = table.Column<string>(nullable: true),
                    TextExperience = table.Column<string>(nullable: true),
                    ExperienceYear = table.Column<long>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    SpecialPrice = table.Column<long>(nullable: true),
                    Departure = table.Column<long>(nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    KidsReception = table.Column<long>(nullable: false),
                    OpinionCount = table.Column<long>(nullable: false),
                    TextAbout = table.Column<string>(nullable: true),
                    TelemedId = table.Column<long>(nullable: true),
                    RatingReviewsLabel = table.Column<string>(nullable: true),
                    IsExclusivePrice = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_Telemeds_TelemedId",
                        column: x => x.TelemedId,
                        principalTable: "Telemeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_HCityId",
                table: "Clinics",
                column: "HCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_HDistrictId",
                table: "Clinics",
                column: "HDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_HStreetId",
                table: "Clinics",
                column: "HStreetId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_ParentId",
                table: "Clinics",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_TelemedId",
                table: "Doctors",
                column: "TelemedId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_BranchId",
                table: "Stations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_CityId",
                table: "Stations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Telemeds_HClinicId",
                table: "Telemeds",
                column: "HClinicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Telemeds");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Districs");

            migrationBuilder.DropTable(
                name: "Streets");
        }
    }
}
