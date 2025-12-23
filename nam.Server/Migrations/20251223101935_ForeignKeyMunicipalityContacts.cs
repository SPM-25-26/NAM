using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyMunicipalityContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityHomeInfos_MunicipalityHomeContactInfos_ContactsId",
                table: "MunicipalityHomeInfos");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityHomeInfos_ContactsId",
                table: "MunicipalityHomeInfos");

            migrationBuilder.DropColumn(
                name: "ContactsId",
                table: "MunicipalityHomeInfos");

            migrationBuilder.AddColumn<string>(
                name: "MunicipalityLegalName",
                table: "MunicipalityHomeContactInfos",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityHomeContactInfos_MunicipalityLegalName",
                table: "MunicipalityHomeContactInfos",
                column: "MunicipalityLegalName",
                unique: true,
                filter: "[MunicipalityLegalName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityHomeContactInfos_MunicipalityHomeInfos_MunicipalityLegalName",
                table: "MunicipalityHomeContactInfos",
                column: "MunicipalityLegalName",
                principalTable: "MunicipalityHomeInfos",
                principalColumn: "LegalName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityHomeContactInfos_MunicipalityHomeInfos_MunicipalityLegalName",
                table: "MunicipalityHomeContactInfos");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityHomeContactInfos_MunicipalityLegalName",
                table: "MunicipalityHomeContactInfos");

            migrationBuilder.DropColumn(
                name: "MunicipalityLegalName",
                table: "MunicipalityHomeContactInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "ContactsId",
                table: "MunicipalityHomeInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityHomeInfos_ContactsId",
                table: "MunicipalityHomeInfos",
                column: "ContactsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityHomeInfos_MunicipalityHomeContactInfos_ContactsId",
                table: "MunicipalityHomeInfos",
                column: "ContactsId",
                principalTable: "MunicipalityHomeContactInfos",
                principalColumn: "Id");
        }
    }
}
