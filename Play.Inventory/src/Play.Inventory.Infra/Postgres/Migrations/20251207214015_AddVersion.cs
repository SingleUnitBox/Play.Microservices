using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Inventory.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastKnownVersion",
                schema: "play.inventory",
                table: "CatalogItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastKnownVersion",
                schema: "play.inventory",
                table: "CatalogItems");
        }
    }
}
