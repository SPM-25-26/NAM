using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class publicEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtCultureNatureDetails_MunicipalityForLocalStorageSetting_MunicipalityDataId",
                table: "ArtCultureNatureDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MunicipalityForLocalStorageSetting",
                table: "MunicipalityForLocalStorageSetting");

            migrationBuilder.RenameTable(
                name: "MunicipalityForLocalStorageSetting",
                newName: "MunicipalityForLocalStorageSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicEventMobileDetailIdentifier",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MunicipalityForLocalStorageSettings",
                table: "MunicipalityForLocalStorageSettings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Organizers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublicEventMobileDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Typology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicEventMobileDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_PublicEventMobileDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PublicEventMobileDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PublicEventMobileDetails_Organizers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Organizers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceSpecificationCurrencyValue = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    ValidityDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidityStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidityEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicEventMobileDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_PublicEventMobileDetails_PublicEventMobileDetailIdentifier",
                        column: x => x.PublicEventMobileDetailIdentifier,
                        principalTable: "PublicEventMobileDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "PublicEventCards",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BadgeText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicEventCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_PublicEventCards_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PublicEventCards_PublicEventMobileDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "PublicEventMobileDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_PublicEventMobileDetailIdentifier",
                table: "FeatureCards",
                column: "PublicEventMobileDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_PublicEventMobileDetailIdentifier",
                table: "Offers",
                column: "PublicEventMobileDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventCards_DetailIdentifier",
                table: "PublicEventCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventCards_MunicipalityDataId",
                table: "PublicEventCards",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventMobileDetails_MunicipalityDataId",
                table: "PublicEventMobileDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventMobileDetails_NearestCarParkId",
                table: "PublicEventMobileDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventMobileDetails_OrganizerId",
                table: "PublicEventMobileDetails",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtCultureNatureDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                table: "ArtCultureNatureDetails",
                column: "MunicipalityDataId",
                principalTable: "MunicipalityForLocalStorageSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_PublicEventMobileDetails_PublicEventMobileDetailIdentifier",
                table: "FeatureCards",
                column: "PublicEventMobileDetailIdentifier",
                principalTable: "PublicEventMobileDetails",
                principalColumn: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtCultureNatureDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                table: "ArtCultureNatureDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_PublicEventMobileDetails_PublicEventMobileDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "PublicEventCards");

            migrationBuilder.DropTable(
                name: "PublicEventMobileDetails");

            migrationBuilder.DropTable(
                name: "Organizers");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_PublicEventMobileDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MunicipalityForLocalStorageSettings",
                table: "MunicipalityForLocalStorageSettings");

            migrationBuilder.DropColumn(
                name: "PublicEventMobileDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.RenameTable(
                name: "MunicipalityForLocalStorageSettings",
                newName: "MunicipalityForLocalStorageSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MunicipalityForLocalStorageSetting",
                table: "MunicipalityForLocalStorageSetting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtCultureNatureDetails_MunicipalityForLocalStorageSetting_MunicipalityDataId",
                table: "ArtCultureNatureDetails",
                column: "MunicipalityDataId",
                principalTable: "MunicipalityForLocalStorageSetting",
                principalColumn: "Id");
        }
    }
}
