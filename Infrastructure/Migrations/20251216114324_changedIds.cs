using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class changedIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtCultureNatureDetails_SiteCards_SiteId",
                table: "ArtCultureNatureDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicEventMobileDetails_Organizers_OrganizerId",
                table: "PublicEventMobileDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteCards",
                table: "SiteCards");

            migrationBuilder.DropIndex(
                name: "IX_PublicEventMobileDetails_OrganizerId",
                table: "PublicEventMobileDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedPois",
                table: "OwnedPois");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizers",
                table: "Organizers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCards",
                table: "FeatureCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssociatedServices",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SiteCards");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "PublicEventMobileDetails");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "OwnedPois");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Organizers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FeatureCards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AssociatedServices");

            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "ArtCultureNatureDetails",
                newName: "SiteIdentifier");

            migrationBuilder.RenameIndex(
                name: "IX_ArtCultureNatureDetails_SiteId",
                table: "ArtCultureNatureDetails",
                newName: "IX_ArtCultureNatureDetails_SiteIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Identifier",
                table: "SiteCards",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "OrganizerTaxCode",
                table: "PublicEventMobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OwnedPois",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "TaxCode",
                table: "Organizers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Identifier",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteCards",
                table: "SiteCards",
                column: "Identifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedPois",
                table: "OwnedPois",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizers",
                table: "Organizers",
                column: "TaxCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCards",
                table: "FeatureCards",
                column: "EntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssociatedServices",
                table: "AssociatedServices",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventMobileDetails_OrganizerTaxCode",
                table: "PublicEventMobileDetails",
                column: "OrganizerTaxCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtCultureNatureDetails_SiteCards_SiteIdentifier",
                table: "ArtCultureNatureDetails",
                column: "SiteIdentifier",
                principalTable: "SiteCards",
                principalColumn: "Identifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicEventMobileDetails_Organizers_OrganizerTaxCode",
                table: "PublicEventMobileDetails",
                column: "OrganizerTaxCode",
                principalTable: "Organizers",
                principalColumn: "TaxCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtCultureNatureDetails_SiteCards_SiteIdentifier",
                table: "ArtCultureNatureDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicEventMobileDetails_Organizers_OrganizerTaxCode",
                table: "PublicEventMobileDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteCards",
                table: "SiteCards");

            migrationBuilder.DropIndex(
                name: "IX_PublicEventMobileDetails_OrganizerTaxCode",
                table: "PublicEventMobileDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedPois",
                table: "OwnedPois");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizers",
                table: "Organizers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeatureCards",
                table: "FeatureCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssociatedServices",
                table: "AssociatedServices");

            migrationBuilder.DropColumn(
                name: "OrganizerTaxCode",
                table: "PublicEventMobileDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OwnedPois");

            migrationBuilder.RenameColumn(
                name: "SiteIdentifier",
                table: "ArtCultureNatureDetails",
                newName: "SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtCultureNatureDetails_SiteIdentifier",
                table: "ArtCultureNatureDetails",
                newName: "IX_ArtCultureNatureDetails_SiteId");

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "SiteCards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SiteCards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizerId",
                table: "PublicEventMobileDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "OwnedPois",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TaxCode",
                table: "Organizers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Organizers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "EntityId",
                table: "FeatureCards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FeatureCards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "AssociatedServices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AssociatedServices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteCards",
                table: "SiteCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedPois",
                table: "OwnedPois",
                column: "Identifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizers",
                table: "Organizers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeatureCards",
                table: "FeatureCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssociatedServices",
                table: "AssociatedServices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEventMobileDetails_OrganizerId",
                table: "PublicEventMobileDetails",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtCultureNatureDetails_SiteCards_SiteId",
                table: "ArtCultureNatureDetails",
                column: "SiteId",
                principalTable: "SiteCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicEventMobileDetails_Organizers_OrganizerId",
                table: "PublicEventMobileDetails",
                column: "OrganizerId",
                principalTable: "Organizers",
                principalColumn: "Id");
        }
    }
}
