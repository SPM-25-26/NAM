using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class artAndArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MobileCategoryDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileCategoryDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityForLocalStorageSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityForLocalStorageSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NearestCarParks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Distance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NearestCarParks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtCultureNatureDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", maxLength: 255, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrimaryImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubjectDiscipline = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Gallery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VirtualTours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NearestCarParkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtCultureNatureDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_ArtCultureNatureDetails_MunicipalityForLocalStorageSetting_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSetting",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArtCultureNatureDetails_NearestCarParks_NearestCarParkId",
                        column: x => x.NearestCarParkId,
                        principalTable: "NearestCarParks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArtCultureNatureDetails_SiteCards_SiteId",
                        column: x => x.SiteId,
                        principalTable: "SiteCards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArtCultureNatureCards",
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
                    table.PrimaryKey("PK_ArtCultureNatureCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ArtCultureNatureCards_ArtCultureNatureDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "AssociatedServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ArtCultureNatureDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociatedServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssociatedServices_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                        column: x => x.ArtCultureNatureDetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "Catalogues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ArtCultureNatureDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalogues_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                        column: x => x.ArtCultureNatureDetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "CreativeWorkMobiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ArtCultureNatureDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreativeWorkMobiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreativeWorkMobiles_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                        column: x => x.ArtCultureNatureDetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "CulturalProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ArtCultureNatureDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CulturalProjects_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                        column: x => x.ArtCultureNatureDetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "CulturalSiteServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ArtCultureNatureDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CulturalSiteServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CulturalSiteServices_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                        column: x => x.ArtCultureNatureDetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "FeatureCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtraInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArtCultureNatureDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCards_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                        column: x => x.ArtCultureNatureDetailIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtCultureNatureCards_DetailIdentifier",
                table: "ArtCultureNatureCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ArtCultureNatureDetails_MunicipalityDataId",
                table: "ArtCultureNatureDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtCultureNatureDetails_NearestCarParkId",
                table: "ArtCultureNatureDetails",
                column: "NearestCarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtCultureNatureDetails_SiteId",
                table: "ArtCultureNatureDetails",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedServices_ArtCultureNatureDetailIdentifier",
                table: "AssociatedServices",
                column: "ArtCultureNatureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogues_ArtCultureNatureDetailIdentifier",
                table: "Catalogues",
                column: "ArtCultureNatureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_CreativeWorkMobiles_ArtCultureNatureDetailIdentifier",
                table: "CreativeWorkMobiles",
                column: "ArtCultureNatureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalProjects_ArtCultureNatureDetailIdentifier",
                table: "CulturalProjects",
                column: "ArtCultureNatureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalSiteServices_ArtCultureNatureDetailIdentifier",
                table: "CulturalSiteServices",
                column: "ArtCultureNatureDetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_ArtCultureNatureDetailIdentifier",
                table: "FeatureCards",
                column: "ArtCultureNatureDetailIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtCultureNatureCards");

            migrationBuilder.DropTable(
                name: "AssociatedServices");

            migrationBuilder.DropTable(
                name: "Catalogues");

            migrationBuilder.DropTable(
                name: "CreativeWorkMobiles");

            migrationBuilder.DropTable(
                name: "CulturalProjects");

            migrationBuilder.DropTable(
                name: "CulturalSiteServices");

            migrationBuilder.DropTable(
                name: "FeatureCards");

            migrationBuilder.DropTable(
                name: "MobileCategoryDetails");

            migrationBuilder.DropTable(
                name: "ArtCultureNatureDetails");

            migrationBuilder.DropTable(
                name: "MunicipalityForLocalStorageSetting");

            migrationBuilder.DropTable(
                name: "NearestCarParks");

            migrationBuilder.DropTable(
                name: "SiteCards");
        }
    }
}
