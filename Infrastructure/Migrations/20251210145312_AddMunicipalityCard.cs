using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMunicipalityCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MunicipalityHomeInfoLegalName",
                table: "FeatureCards",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MunicipalityHomeInfoLegalName1",
                table: "FeatureCards",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MunicipalityHomeContactInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityHomeContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityHomeInfos",
                columns: table => new
                {
                    LegalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ContactsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HomeImages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanoramaPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PanoramaWidth = table.Column<int>(type: "int", nullable: true),
                    VirtualTourUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAndProvince = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityHomeInfos", x => x.LegalName);
                    table.ForeignKey(
                        name: "FK_MunicipalityHomeInfos_MunicipalityHomeContactInfos_ContactsId",
                        column: x => x.ContactsId,
                        principalTable: "MunicipalityHomeContactInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityCards",
                columns: table => new
                {
                    LegalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DetailLegalName = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityCards", x => x.LegalName);
                    table.ForeignKey(
                        name: "FK_MunicipalityCards_MunicipalityHomeInfos_DetailLegalName",
                        column: x => x.DetailLegalName,
                        principalTable: "MunicipalityHomeInfos",
                        principalColumn: "LegalName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName1");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityCards_DetailLegalName",
                table: "MunicipalityCards",
                column: "DetailLegalName");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityHomeInfos_ContactsId",
                table: "MunicipalityHomeInfos",
                column: "ContactsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName",
                principalTable: "MunicipalityHomeInfos",
                principalColumn: "LegalName");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName1",
                principalTable: "MunicipalityHomeInfos",
                principalColumn: "LegalName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName",
                table: "FeatureCards");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards");

            migrationBuilder.DropTable(
                name: "MunicipalityCards");

            migrationBuilder.DropTable(
                name: "MunicipalityHomeInfos");

            migrationBuilder.DropTable(
                name: "MunicipalityHomeContactInfos");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "MunicipalityHomeInfoLegalName",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "MunicipalityHomeInfoLegalName1",
                table: "FeatureCards");
        }
    }
}
