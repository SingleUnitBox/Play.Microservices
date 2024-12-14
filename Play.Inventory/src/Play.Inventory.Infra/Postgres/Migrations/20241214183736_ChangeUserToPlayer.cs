using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Inventory.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "play.inventory",
                table: "MoneyBags",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "play.inventory",
                table: "InventoryItems",
                newName: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerId",
                schema: "play.inventory",
                table: "MoneyBags",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                schema: "play.inventory",
                table: "InventoryItems",
                newName: "UserId");
        }
    }
}
