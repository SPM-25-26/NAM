using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class PublicEventRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_PublicEventMobileDetails_PublicEventMobileDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_PublicEventMobileDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "PublicEventMobileDetailIdentifier",
                table: "FeatureCards");

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<PublicEventMobileDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<PublicEventMobileDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<PublicEventMobileDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<PublicEventMobileDetail>_PublicEventMobileDetails_RelatedEntityIdentifier",
                        column: x => x.RelatedEntityIdentifier,
                        principalTable: "PublicEventMobileDetails",
                        principalColumn: "Identifier");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<PublicEventMobileDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<PublicEventMobileDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<PublicEventMobileDetail>_RelatedEntityIdentifier",
                table: "FeatureCardRelationship<PublicEventMobileDetail>",
                column: "RelatedEntityIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<PublicEventMobileDetail>");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicEventMobileDetailIdentifier",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_PublicEventMobileDetailIdentifier",
                table: "FeatureCards",
                column: "PublicEventMobileDetailIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_PublicEventMobileDetails_PublicEventMobileDetailIdentifier",
                table: "FeatureCards",
                column: "PublicEventMobileDetailIdentifier",
                principalTable: "PublicEventMobileDetails",
                principalColumn: "Identifier");
        }
    }
}
