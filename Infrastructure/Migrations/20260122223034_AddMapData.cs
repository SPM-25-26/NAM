using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMapData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapData",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CentreLatitude = table.Column<double>(type: "float", nullable: false),
                    CentreLongitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapData", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "MapMarkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Typology = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MapDataName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapMarkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapMarkers_MapData_MapDataName",
                        column: x => x.MapDataName,
                        principalTable: "MapData",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapMarkers_MapDataName",
                table: "MapMarkers",
                column: "MapDataName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapMarkers");

            migrationBuilder.DropTable(
                name: "MapData");
        }
    }
}
