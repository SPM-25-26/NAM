using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class MunicipalityRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName",
                table: "FeatureCards");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName",
                table: "FeatureCards");

            migrationBuilder.DropIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "MunicipalityHomeInfoLegalName",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "MunicipalityHomeInfoLegalName1",
                table: "FeatureCards");

            migrationBuilder.CreateTable(
                name: "FeatureCardRelationship<MunicipalityHomeInfo>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCardEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityLegalName = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    MunicipalityHomeInfoLegalName = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    MunicipalityHomeInfoLegalName1 = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCardRelationship<MunicipalityHomeInfo>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<MunicipalityHomeInfo>_FeatureCards_FeatureCardEntityId",
                        column: x => x.FeatureCardEntityId,
                        principalTable: "FeatureCards",
                        principalColumn: "EntityId");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<MunicipalityHomeInfo>_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName",
                        column: x => x.MunicipalityHomeInfoLegalName,
                        principalTable: "MunicipalityHomeInfos",
                        principalColumn: "LegalName");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<MunicipalityHomeInfo>_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName1",
                        column: x => x.MunicipalityHomeInfoLegalName1,
                        principalTable: "MunicipalityHomeInfos",
                        principalColumn: "LegalName");
                    table.ForeignKey(
                        name: "FK_FeatureCardRelationship<MunicipalityHomeInfo>_MunicipalityHomeInfos_RelatedEntityLegalName",
                        column: x => x.RelatedEntityLegalName,
                        principalTable: "MunicipalityHomeInfos",
                        principalColumn: "LegalName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<MunicipalityHomeInfo>_FeatureCardEntityId",
                table: "FeatureCardRelationship<MunicipalityHomeInfo>",
                column: "FeatureCardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<MunicipalityHomeInfo>_MunicipalityHomeInfoLegalName",
                table: "FeatureCardRelationship<MunicipalityHomeInfo>",
                column: "MunicipalityHomeInfoLegalName");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<MunicipalityHomeInfo>_MunicipalityHomeInfoLegalName1",
                table: "FeatureCardRelationship<MunicipalityHomeInfo>",
                column: "MunicipalityHomeInfoLegalName1");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCardRelationship<MunicipalityHomeInfo>_RelatedEntityLegalName",
                table: "FeatureCardRelationship<MunicipalityHomeInfo>",
                column: "RelatedEntityLegalName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCardRelationship<MunicipalityHomeInfo>");

            migrationBuilder.AddColumn<string>(
                name: "MunicipalityHomeInfoLegalName",
                table: "FeatureCards",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MunicipalityHomeInfoLegalName1",
                table: "FeatureCards",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCards_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName1");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName",
                principalTable: "MunicipalityHomeInfos",
                principalColumn: "LegalName");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureCards_MunicipalityHomeInfos_MunicipalityHomeInfoLegalName1",
                table: "FeatureCards",
                column: "MunicipalityHomeInfoLegalName1",
                principalTable: "MunicipalityHomeInfos",
                principalColumn: "LegalName");
        }
    }
}
