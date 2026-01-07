using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Play.World.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Zone",
                schema: "play.world",
                table: "Zone");

            migrationBuilder.RenameTable(
                name: "Zone",
                schema: "play.world",
                newName: "Zones",
                newSchema: "play.world");

            migrationBuilder.AddColumn<Polygon>(
                name: "Boundary",
                schema: "play.world",
                table: "Zones",
                type: "geometry(Polygon, 4326)",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "play.world",
                table: "Zones",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zones",
                schema: "play.world",
                table: "Zones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_Boundary",
                schema: "play.world",
                table: "Zones",
                column: "Boundary")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Zones",
                schema: "play.world",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_Boundary",
                schema: "play.world",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "Boundary",
                schema: "play.world",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "play.world",
                table: "Zones");

            migrationBuilder.RenameTable(
                name: "Zones",
                schema: "play.world",
                newName: "Zone",
                newSchema: "play.world");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zone",
                schema: "play.world",
                table: "Zone",
                column: "Id");
        }
    }
}
