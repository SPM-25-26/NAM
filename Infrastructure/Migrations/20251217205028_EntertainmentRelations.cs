using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class EntertainmentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_EntertainmentLeisureDetails_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<EntertainmentLeisureDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<EntertainmentLeisureDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<EntertainmentLeisureDetail>_EntertainmentLeisureDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "EntertainmentLeisureDetails",
                        principalColumn: "Identifier");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<EntertainmentLeisureDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<EntertainmentLeisureDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<EntertainmentLeisureDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<EntertainmentLeisureDetail>_RelatedEntityIdentifier",
                table: "FeatureCardRelationship<EntertainmentLeisureDetail>",
                column: "RelatedEntityIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<EntertainmentLeisureDetail>");

            migrationBuilder.AddColumn<Guid>(
                name: "EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards",
                column: "EntertainmentLeisureDetailIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_EntertainmentLeisureDetails_EntertainmentLeisureDetailIdentifier",
                table: "FeatureCards",
                column: "EntertainmentLeisureDetailIdentifier",
                principalTable: "EntertainmentLeisureDetails",
                principalColumn: "Identifier");
        }
    }
}
