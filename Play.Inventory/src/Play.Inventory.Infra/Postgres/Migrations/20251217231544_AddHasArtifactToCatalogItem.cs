using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Inventory.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddHasArtifactToCatalogItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasArtifact",
                schema: "play.inventory",
                table: "CatalogItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasArtifact",
                schema: "play.inventory",
                table: "CatalogItems");
        }
    }
}
