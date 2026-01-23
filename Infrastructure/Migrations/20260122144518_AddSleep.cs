using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSleep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SleepCardDetailIdentifier",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SleepCardDetailIdentifier",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SleepDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Typology = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PrimaryImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Services = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomTypologies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    OwnerTaxCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Booking_TimeIntervalDto_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Booking_TimeIntervalDto_StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Booking_TimeIntervalDto_EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Booking_Name = table.Column<int>(type: "int", nullable: true),
                    Booking_Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShortAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SleepDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_SleepDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SleepDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SleepDetails_Owners_OwnerTaxCode",
                        column: x => x.OwnerTaxCode,
                        principalTable: "Owners",
                        principalColumn: "TaxCode");
                });

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<SleepCardDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<SleepCardDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<SleepCardDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<SleepCardDetail>_SleepDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "SleepDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "SleepCards",
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
                    table.PrimaryKey("PK_SleepCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_SleepCards_SleepDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "SleepDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_SleepCardDetailIdentifier",
                table: "Offers",
                column: "SleepCardDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedServices_SleepCardDetailIdentifier",
                table: "AssociatedServices",
                column: "SleepCardDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<SleepCardDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<SleepCardDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<SleepCardDetail>_RelatedEntityIdentifier",
                table: "FeatureCardRelationship<SleepCardDetail>",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_SleepCards_DetailIdentifier",
                table: "SleepCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_SleepDetails_MunicipalityDataId",
                table: "SleepDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_SleepDetails_NearestCarParkId",
                table: "SleepDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_SleepDetails_OwnerTaxCode",
                table: "SleepDetails",
                column: "OwnerTaxCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedServices_SleepDetails_SleepCardDetailIdentifier",
                table: "AssociatedServices",
                column: "SleepCardDetailIdentifier",
                principalTable: "SleepDetails",
                principalColumn: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_SleepDetails_SleepCardDetailIdentifier",
                table: "Offers",
                column: "SleepCardDetailIdentifier",
                principalTable: "SleepDetails",
                principalColumn: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedServices_SleepDetails_SleepCardDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_SleepDetails_SleepCardDetailIdentifier",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<SleepCardDetail>");

            migrationBuilder.DropTable(
                name: "SleepCards");

            migrationBuilder.DropTable(
                name: "SleepDetails");

            migrationBuilder.DropIndex(
                name: "IX_Offers_SleepCardDetailIdentifier",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedServices_SleepCardDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "SleepCardDetailIdentifier",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "SleepCardDetailIdentifier",
                table: "AssociatedServices");
        }
    }
}
