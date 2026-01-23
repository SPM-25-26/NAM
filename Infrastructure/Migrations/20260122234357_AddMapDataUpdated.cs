using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nam.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMapDataUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CentreLongitude",
                table: "MapData",
                newName: "CenterLongitude");

            migrationBuilder.RenameColumn(
                name: "CentreLatitude",
                table: "MapData",
                newName: "CenterLatitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CenterLongitude",
                table: "MapData",
                newName: "CentreLongitude");

            migrationBuilder.RenameColumn(
                name: "CenterLatitude",
                table: "MapData",
                newName: "CentreLatitude");
        }
    }
}
