using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.World.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "position",
                schema: "play.world",
                table: "ItemLocations",
                newName: "Position");

            migrationBuilder.RenameIndex(
                name: "IX_ItemLocations_position",
                schema: "play.world",
                table: "ItemLocations",
                newName: "IX_ItemLocations_Position");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                schema: "play.world",
                table: "ItemLocations",
                newName: "position");

            migrationBuilder.RenameIndex(
                name: "IX_ItemLocations_Position",
                schema: "play.world",
                table: "ItemLocations",
                newName: "IX_ItemLocations_position");
        }
    }
}
