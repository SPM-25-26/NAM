using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddShopping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCardDetailIdentifier",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    TaxCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.TaxCode);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PoiCategory = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerTaxCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningHours_Opens = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_Closes = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OpeningHours_AdmissionType_Name = table.Column<int>(type: "int", nullable: true),
                    OpeningHours_AdmissionType_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OpeningHours_TimeInterval_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_TimeInterval_StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_TimeInterval_EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_Day = table.Column<int>(type: "int", nullable: true),
                    TemporaryClosure_ReasonForClosure = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TemporaryClosure_Opens = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemporaryClosure_Closes = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemporaryClosure_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TemporaryClosure_TimeInterval_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemporaryClosure_TimeInterval_StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemporaryClosure_TimeInterval_EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemporaryClosure_Day = table.Column<int>(type: "int", nullable: true),
                    Booking_TimeIntervalDto_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Booking_TimeIntervalDto_StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Booking_TimeIntervalDto_EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Booking_Name = table.Column<int>(type: "int", nullable: true),
                    Booking_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_ShoppingDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingDetails_Owners_OwnerTaxCode",
                        column: x => x.OwnerTaxCode,
                        principalTable: "Owners",
                        principalColumn: "TaxCode");
                });

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<ShoppingCardDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<ShoppingCardDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<ShoppingCardDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<ShoppingCardDetail>_ShoppingDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "ShoppingDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "PointOfSaleService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShoppingCardDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOfSaleService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointOfSaleService_ShoppingDetails_ShoppingCardDetailIdentifier",
                        column: x => x.ShoppingCardDetailIdentifier,
                        principalTable: "ShoppingDetails",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCards",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BadgeText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ShoppingCards_ShoppingDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "ShoppingDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "TypicalProducts",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Certification = table.Column<int>(type: "int", nullable: true),
                    ShoppingCardDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypicalProducts", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_TypicalProducts_ShoppingDetails_ShoppingCardDetailIdentifier",
                        column: x => x.ShoppingCardDetailIdentifier,
                        principalTable: "ShoppingDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedServices_ShoppingCardDetailIdentifier",
                table: "AssociatedServices",
                column: "ShoppingCardDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<ShoppingCardDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<ShoppingCardDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<ShoppingCardDetail>_RelatedEntityIdentifier",
                table: "FeatureCardRelationship<ShoppingCardDetail>",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfSaleService_ShoppingCardDetailIdentifier",
                table: "PointOfSaleService",
                column: "ShoppingCardDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCards_DetailIdentifier",
                table: "ShoppingCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingDetails_MunicipalityDataId",
                table: "ShoppingDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingDetails_NearestCarParkId",
                table: "ShoppingDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingDetails_OwnerTaxCode",
                table: "ShoppingDetails",
                column: "OwnerTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_TypicalProducts_ShoppingCardDetailIdentifier",
                table: "TypicalProducts",
                column: "ShoppingCardDetailIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedServices_ShoppingDetails_ShoppingCardDetailIdentifier",
                table: "AssociatedServices",
                column: "ShoppingCardDetailIdentifier",
                principalTable: "ShoppingDetails",
                principalColumn: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedServices_ShoppingDetails_ShoppingCardDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<ShoppingCardDetail>");

            migrationBuilder.DropTable(
                name: "PointOfSaleService");

            migrationBuilder.DropTable(
                name: "ShoppingCards");

            migrationBuilder.DropTable(
                name: "TypicalProducts");

            migrationBuilder.DropTable(
                name: "ShoppingDetails");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedServices_ShoppingCardDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "ShoppingCardDetailIdentifier",
                table: "AssociatedServices");
        }
    }
}
