using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_OrganizationMobileDetailTaxCode",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "OrganizationMobileDetailTaxCode",
                table: "FeatureCards");

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<OrganizationMobileDetail>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityTaxCode = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<OrganizationMobileDetail>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<OrganizationMobileDetail>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<OrganizationMobileDetail>_OrganizationMobileDetails_RelatedEntityTaxCode",
                        column: x => x.RelatedEntityTaxCode,
                        principalTable: "OrganizationMobileDetails",
                        principalColumn: "TaxCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<OrganizationMobileDetail>_FeatureCardEntityId",
                table: "FeatureCardRelationship<OrganizationMobileDetail>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<OrganizationMobileDetail>_RelatedEntityTaxCode",
                table: "FeatureCardRelationship<OrganizationMobileDetail>",
                column: "RelatedEntityTaxCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<OrganizationMobileDetail>");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationMobileDetailTaxCode",
                table: "FeatureCards",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_OrganizationMobileDetailTaxCode",
                table: "FeatureCards",
                column: "OrganizationMobileDetailTaxCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_OrganizationMobileDetails_OrganizationMobileDetailTaxCode",
                table: "FeatureCards",
                column: "OrganizationMobileDetailTaxCode",
                principalTable: "OrganizationMobileDetails",
                principalColumn: "TaxCode");
        }
    }
}
