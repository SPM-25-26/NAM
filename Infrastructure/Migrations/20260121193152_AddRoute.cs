using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoutePoints",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePoints", x => x.Address);
                });

            migrationBuilder.CreateTable(
                name: "StageMobiles",
                columns: table => new
                {
                    PoiIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PoiOfficialName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PoiImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PoiImageThumbPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Signposting = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SupportService = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PoiLatitude = table.Column<double>(type: "float", nullable: false),
                    PoiLongitude = table.Column<double>(type: "float", nullable: false),
                    PoiAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageMobiles", x => x.PoiIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "RouteDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PathTheme = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TravellingMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrganizationWebsite = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrganizationEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    OrganizationFacebook = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrganizationInstagram = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrganizationTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SecurityLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumberOfStages = table.Column<int>(type: "int", nullable: false),
                    QuantifiedPathwayPaving = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RouteLength = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartingPointAddress = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BestWhen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_RouteDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteDetails_RoutePoints_StartingPointAddress",
                        column: x => x.StartingPointAddress,
                        principalTable: "RoutePoints",
                        principalColumn: "Address");
                });

            migrationBuilder.CreateTable(
                name: "RouteCards",
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
                    table.PrimaryKey("PK_RouteCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_RouteCards_RouteDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "RouteDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "RouteFeatureCardRelationships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteFeatureCardRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteFeatureCardRelationships_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_RouteFeatureCardRelationships_RouteDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "RouteDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "RouteStageMobileRelationships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageMobilePoiIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStageMobileRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStageMobileRelationships_RouteDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "RouteDetails",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_RouteStageMobileRelationships_StageMobiles_StageMobilePoiIdentifier",
                        column: x => x.StageMobilePoiIdentifier,
                        principalTable: "StageMobiles",
                        principalColumn: "PoiIdentifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteCards_DetailIdentifier",
                table: "RouteCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RouteDetails_MunicipalityDataId",
                table: "RouteDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteDetails_StartingPointAddress",
                table: "RouteDetails",
                column: "StartingPointAddress");

            migrationBuilder.CreateIndex(
                name: "IX_RouteFeatureCardRelationships_FeatureCardEntityId",
                table: "RouteFeatureCardRelationships",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteFeatureCardRelationships_RelatedEntityIdentifier",
                table: "RouteFeatureCardRelationships",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageMobileRelationships_RelatedEntityIdentifier",
                table: "RouteStageMobileRelationships",
                column: "RelatedEntityIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageMobileRelationships_StageMobilePoiIdentifier",
                table: "RouteStageMobileRelationships",
                column: "StageMobilePoiIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteCards");

            migrationBuilder.DropTable(
                name: "RouteFeatureCardRelationships");

            migrationBuilder.DropTable(
                name: "RouteStageMobileRelationships");

            migrationBuilder.DropTable(
                name: "RouteDetails");

            migrationBuilder.DropTable(
                name: "StageMobiles");

            migrationBuilder.DropTable(
                name: "RoutePoints");
        }
    }
}
