using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class addArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleDetails",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Script = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Subtitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TimeToRead = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Themes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MunicipalityDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleDetails", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_ArticleDetails_MunicipalityForLocalStorageSettings_MunicipalityDataId",
                        column: x => x.MunicipalityDataId,
                        principalTable: "MunicipalityForLocalStorageSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticleCards",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BadgeText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCards", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ArticleCards_ArticleDetails_DetailIdentifier",
                        column: x => x.DetailIdentifier,
                        principalTable: "ArticleDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateTable(
                name: "Paragraphs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleDetailIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Script = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReferenceCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferenceLatitude = table.Column<double>(type: "float", nullable: true),
                    ReferenceLongitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraphs_ArticleDetails_ArticleDetailIdentifier",
                        column: x => x.ArticleDetailIdentifier,
                        principalTable: "ArticleDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCards_DetailIdentifier",
                table: "ArticleCards",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleDetails_MunicipalityDataId",
                table: "ArticleDetails",
                column: "MunicipalityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_ArticleDetailIdentifier",
                table: "Paragraphs",
                column: "ArticleDetailIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleCards");

            migrationBuilder.DropTable(
                name: "Paragraphs");

            migrationBuilder.DropTable(
                name: "ArticleDetails");
        }
    }
}
