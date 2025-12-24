using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationMobileDetailTaxCode",
                table: "PublicEventCards",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationMobileDetailTaxCode",
                table: "Offers",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoPath",
                table: "MunicipalityForLocalStorageSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationMobileDetailTaxCode",
                table: "FeatureCards",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizationMobileDetails",
                columns: table => new
                {
                    TaxCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrimaryImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    MainFunction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FoundationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LegalStatus = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMobileDetails", x => x.TaxCode);
                    table.ForeignKey(
                        name: "FK_OrganizationMobileDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrganizationMobileDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrganizationCards",
                columns: table => new
                {
                    TaxCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BadgeText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DetailTaxCode = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCards", x => x.TaxCode);
                    table.ForeignKey(
                        name: "FK_OrganizationCards_OrganizationMobileDetails_DetailTaxCode",
                        column: x => x.DetailTaxCode,
                        principalTable: "OrganizationMobileDetails",
                        principalColumn: "TaxCode");
                });

            migrationBuilder.CreateTable(
                name: "OwnedPois",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrganizationMobileDetailTaxCode = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedPois", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_OwnedPois_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                        column: x => x.OrganizationMobileDetailTaxCode,
                        principalTable: "OrganizationMobileDetails",
                        principalColumn: "TaxCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventCards_OrganizationMobileDetailTaxCode",
                table: "PublicEventCards",
                column: "OrganizationMobileDetailTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OrganizationMobileDetailTaxCode",
                table: "Offers",
                column: "OrganizationMobileDetailTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_OrganizationMobileDetailTaxCode",
                table: "FeatureCards",
                column: "OrganizationMobileDetailTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationCards_DetailTaxCode",
                table: "OrganizationCards",
                column: "DetailTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMobileDetails_MunicipalityDataId",
                table: "OrganizationMobileDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMobileDetails_NearestCarParkId",
                table: "OrganizationMobileDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnedPois_OrganizationMobileDetailTaxCode",
                table: "OwnedPois",
                column: "OrganizationMobileDetailTaxCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "FeatureCards",
                column: "OrganizationMobileDetailTaxCode",
                principalTable: "OrganizationMobileDetails",
                principalColumn: "TaxCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "Offers",
                column: "OrganizationMobileDetailTaxCode",
                principalTable: "OrganizationMobileDetails",
                principalColumn: "TaxCode");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicEventCards_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "PublicEventCards",
                column: "OrganizationMobileDetailTaxCode",
                principalTable: "OrganizationMobileDetails",
                principalColumn: "TaxCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "FeatureCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicEventCards_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "PublicEventCards");

            migrationBuilder.DropTable(
                name: "OrganizationCards");

            migrationBuilder.DropTable(
                name: "OwnedPois");

            migrationBuilder.DropTable(
                name: "OrganizationMobileDetails");

            migrationBuilder.DropIndex(
                name: "IX_PublicEventCards_OrganizationMobileDetailTaxCode",
                table: "PublicEventCards");

            migrationBuilder.DropIndex(
                name: "IX_Offers_OrganizationMobileDetailTaxCode",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_OrganizationMobileDetailTaxCode",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "OrganizationMobileDetailTaxCode",
                table: "PublicEventCards");

            migrationBuilder.DropColumn(
                name: "OrganizationMobileDetailTaxCode",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OrganizationMobileDetailTaxCode",
                table: "FeatureCards");

            migrationBuilder.AlterColumn<string>(
                name: "LogoPath",
                table: "MunicipalityForLocalStorageSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
