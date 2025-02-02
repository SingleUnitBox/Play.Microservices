using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddCrafterName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "play.items",
                table: "Crafters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "play.items",
                table: "Crafters");
        }
    }
}
