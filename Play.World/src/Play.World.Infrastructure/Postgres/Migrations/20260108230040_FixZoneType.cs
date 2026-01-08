using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.World.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class FixZoneType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "play.world",
                table: "Zones",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "play.world",
                table: "Zones");
        }
    }
}
