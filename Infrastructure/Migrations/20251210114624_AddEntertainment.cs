using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEntertainment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntertainmentLeisureDetailIdentifier",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EntertainmentLeisureDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrimaryImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentLeisureDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_EntertainmentLeisureDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntertainmentLeisureDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentLeisureCards",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BadgeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentLeisureCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_EntertainmentLeisureCards_EntertainmentLeisureDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "EntertainmentLeisureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards",
                column: "EntertainmentLeisureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedServices_EntertainmentLeisureDetailIdentifier",
                table: "AssociatedServices",
                column: "EntertainmentLeisureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentLeisureCards_DetailIdentifier",
                table: "EntertainmentLeisureCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentLeisureDetails_MunicipalityDataId",
                table: "EntertainmentLeisureDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentLeisureDetails_NearestCarParkId",
                table: "EntertainmentLeisureDetails",
                column: "NearestCarParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedServices_EntertainmentLeisureDetails_EntertainmentLeisureDetailIdentifier",
                table: "AssociatedServices",
                column: "EntertainmentLeisureDetailIdentifier",
                principalTable: "EntertainmentLeisureDetails",
                principalColumn: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_EntertainmentLeisureDetails_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards",
                column: "EntertainmentLeisureDetailIdentifier",
                principalTable: "EntertainmentLeisureDetails",
                principalColumn: "Identifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedServices_EntertainmentLeisureDetails_EntertainmentLeisureDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_EntertainmentLeisureDetails_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropTable(
                name: "EntertainmentLeisureCards");

            migrationBuilder.DropTable(
                name: "EntertainmentLeisureDetails");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_AssociatedServices_EntertainmentLeisureDetailIdentifier",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "EntertainmentLeisureDetailIdentifier",
                table: "AssociatedServices");
        }
    }
}
