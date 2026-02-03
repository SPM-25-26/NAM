using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEatAndDrink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EatAndDrinkDetailIdentifier",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EatAndDrinkDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    PrimaryImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OfficialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OpeningHours_Opens = table.Column<TimeOnly>(type: "time", nullable: true),
                    OpeningHours_Closes = table.Column<TimeOnly>(type: "time", nullable: true),
                    OpeningHours_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OpeningHours_AdmissionType_Name = table.Column<int>(type: "int", nullable: true),
                    OpeningHours_AdmissionType_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OpeningHours_TimeInterval_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_TimeInterval_StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_TimeInterval_EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningHours_Day = table.Column<int>(type: "int", nullable: true),
                    TemporaryClosure_ReasonForClosure = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TemporaryClosure_Opens = table.Column<TimeOnly>(type: "time", nullable: true),
                    TemporaryClosure_Closes = table.Column<TimeOnly>(type: "time", nullable: true),
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
                    DietaryNeeds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerTaxCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EatAndDrinkDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_EatAndDrinkDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EatAndDrinkDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EatAndDrinkDetails_Owners_OwnerTaxCode",
                        column: x => x.OwnerTaxCode,
                        principalTable: "Owners",
                        principalColumn: "TaxCode");
                });

            migrationBuilder.CreateTable(
                name: "EatAndDrinkCards",
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
                    table.PrimaryKey("PK_EatAndDrinkCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_EatAndDrinkCards_EatAndDrinkDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "EatAndDrinkDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<EatAndDrinkDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<EatAndDrinkDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<EatAndDrinkDetail>_EatAndDrinkDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "EatAndDrinkDetails",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<EatAndDrinkDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "OntoremaService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EatAndDrinkDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OntoremaService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OntoremaService_EatAndDrinkDetails_EatAndDrinkDetailIdentifier",
                        column: x => x.EatAndDrinkDetailIdentifier,
                        principalTable: "EatAndDrinkDetails",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypicalProductMobile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EatAndDrinkDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypicalProductMobile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypicalProductMobile_EatAndDrinkDetails_EatAndDrinkDetailIdentifier",
                        column: x => x.EatAndDrinkDetailIdentifier,
                        principalTable: "EatAndDrinkDetails",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedServices_EatAndDrinkDetailIdentifier",
                table: "AssociatedServices",
                column: "EatAndDrinkDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EatAndDrinkCards_DetailIdentifier",
                table: "EatAndDrinkCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EatAndDrinkDetails_MunicipalityDataId",
                table: "EatAndDrinkDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_EatAndDrinkDetails_NearestCarParkId",
                table: "EatAndDrinkDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_EatAndDrinkDetails_OwnerTaxCode",
                table: "EatAndDrinkDetails",
                column: "OwnerTaxCode");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<EatAndDrinkDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<EatAndDrinkDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<EatAndDrinkDetail>_RelatedEntityIdentifier",
                table: "FeatureCardRelationship<EatAndDrinkDetail>",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_OntoremaService_EatAndDrinkDetailIdentifier",
                table: "OntoremaService",
                column: "EatAndDrinkDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_TypicalProductMobile_EatAndDrinkDetailIdentifier",
                table: "TypicalProductMobile",
                column: "EatAndDrinkDetailIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedServices_EatAndDrinkDetails_EatAndDrinkDetailIdentifier",
                table: "AssociatedServices",
                column: "EatAndDrinkDetailIdentifier",
                principalTable: "EatAndDrinkDetails",
                principalColumn: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedServices_EatAndDrinkDetails_EatAndDrinkDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropTable(
                name: "EatAndDrinkCards");

            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<EatAndDrinkDetail>");

            migrationBuilder.DropTable(
                name: "OntoremaService");

            migrationBuilder.DropTable(
                name: "TypicalProductMobile");

            migrationBuilder.DropTable(
                name: "EatAndDrinkDetails");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedServices_EatAndDrinkDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "EatAndDrinkDetailIdentifier",
                table: "AssociatedServices");
        }
    }
}
