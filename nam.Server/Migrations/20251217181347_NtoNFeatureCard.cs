using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class NtoNFeatureCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_ArtCultureNatureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "ArtCultureNatureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.CreateTable(
                name: "FeatureCardArtCultureRelationships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardArtCultureRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardArtCultureRelationships_ArtCultureNatureDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "ArtCultureNatureDetails",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_FeatureCardArtCultureRelationships_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardArtCultureRelationships_FeatureCardEntityId",
                table: "FeatureCardArtCultureRelationships",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardArtCultureRelationships_RelatedEntityIdentifier",
                table: "FeatureCardArtCultureRelationships",
                column: "RelatedEntityIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCardArtCultureRelationships");

            migrationBuilder.AddColumn<Guid>(
                name: "ArtCultureNatureDetailIdentifier",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_ArtCultureNatureDetailIdentifier",
                table: "FeatureCards",
                column: "ArtCultureNatureDetailIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_ArtCultureNatureDetails_ArtCultureNatureDetailIdentifier",
                table: "FeatureCards",
                column: "ArtCultureNatureDetailIdentifier",
                principalTable: "ArtCultureNatureDetails",
                principalColumn: "Identifier");
        }
    }
}
