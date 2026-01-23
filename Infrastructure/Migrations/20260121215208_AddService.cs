using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceDetailIdentifier",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    SpacesForDisabled = table.Column<int>(type: "int", nullable: true),
                    PayingParkingSpaces = table.Column<int>(type: "int", nullable: true),
                    AvailableParkingSpaces = table.Column<int>(type: "int", nullable: true),
                    PostiAutoVenduti = table.Column<int>(type: "int", nullable: true),
                    TotalNumberOfCarParkSpaces = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Typology = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PrimaryImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReservationUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_ServiceDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_ServiceDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceLocation",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLocation", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<ServiceDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<ServiceDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<ServiceDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<ServiceDetail>_ServiceDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "ServiceDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "ServiceCards",
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
                    table.PrimaryKey("PK_ServiceCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ServiceCards_ServiceDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "ServiceDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "ServiceLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceLocationIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLocations_ServiceDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "ServiceDetails",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_ServiceLocations_ServiceLocation_ServiceLocationIdentifier",
                        column: x => x.ServiceLocationIdentifier,
                        principalTable: "ServiceLocation",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedServices_ServiceDetailIdentifier",
                table: "AssociatedServices",
                column: "ServiceDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<ServiceDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<ServiceDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<ServiceDetail>_RelatedEntityIdentifier",
                table: "FeatureCardRelationship<ServiceDetail>",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCards_DetailIdentifier",
                table: "ServiceCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDetails_MunicipalityDataId",
                table: "ServiceDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDetails_NearestCarParkId",
                table: "ServiceDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLocations_RelatedEntityIdentifier",
                table: "ServiceLocations",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLocations_ServiceLocationIdentifier",
                table: "ServiceLocations",
                column: "ServiceLocationIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedServices_ServiceDetails_ServiceDetailIdentifier",
                table: "AssociatedServices",
                column: "ServiceDetailIdentifier",
                principalTable: "ServiceDetails",
                principalColumn: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedServices_ServiceDetails_ServiceDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<ServiceDetail>");

            migrationBuilder.DropTable(
                name: "ServiceCards");

            migrationBuilder.DropTable(
                name: "ServiceLocations");

            migrationBuilder.DropTable(
                name: "ServiceDetails");

            migrationBuilder.DropTable(
                name: "ServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedServices_ServiceDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "ServiceDetailIdentifier",
                table: "AssociatedServices");
        }
    }
}
