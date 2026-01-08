using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuestionaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionaireId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Questionaire",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Interest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelStyle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgeRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelCompanions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscoveryMode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionaire", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_QuestionaireId",
                table: "Users",
                column: "QuestionaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Questionaire_QuestionaireId",
                table: "Users",
                column: "QuestionaireId",
                principalTable: "Questionaire",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Questionaire_QuestionaireId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Questionaire");

            migrationBuilder.DropIndex(
                name: "IX_Users_QuestionaireId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QuestionaireId",
                table: "Users");
        }
    }
}
