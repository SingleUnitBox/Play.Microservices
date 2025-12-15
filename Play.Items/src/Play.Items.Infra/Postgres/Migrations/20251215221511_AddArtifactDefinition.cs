using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddArtifactDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("149fa849-be35-40af-b8ae-b1079eb29c52"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("19c4d812-4e36-453d-a562-ce618c7bae55"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("437da457-5951-4826-bc84-e4c8d3d45ec5"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("6794bc09-2099-4fe0-bf8c-32deb9d719bd"));

            migrationBuilder.AddColumn<string>(
                name: "Socket_Artifact_CompatibleHollow",
                schema: "play.items",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Socket_Artifact_Stats",
                schema: "play.items",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Socket_HollowType",
                schema: "play.items",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArtifactDefinitions",
                schema: "play.items",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompatibleHollowType = table.Column<string>(type: "text", nullable: false),
                    BaseStats = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactDefinitions", x => x.Name);
                });

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Elements",
                columns: new[] { "ElementId", "ElementName" },
                values: new object[,]
                {
                    { new Guid("0c877047-800a-4495-ae59-e3ab6723aaef"), "Water" },
                    { new Guid("7b79fb39-ed40-48ca-afad-9af5d6aee8e8"), "Fire" },
                    { new Guid("8d575c45-6ec0-47a6-808a-acedc26a7607"), "Wind" },
                    { new Guid("a079e1f7-6f4c-47b1-898e-75c51c0dfa7b"), "Earth" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtifactDefinitions",
                schema: "play.items");

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("0c877047-800a-4495-ae59-e3ab6723aaef"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("7b79fb39-ed40-48ca-afad-9af5d6aee8e8"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("8d575c45-6ec0-47a6-808a-acedc26a7607"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("a079e1f7-6f4c-47b1-898e-75c51c0dfa7b"));

            migrationBuilder.DropColumn(
                name: "Socket_Artifact_CompatibleHollow",
                schema: "play.items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Socket_Artifact_Stats",
                schema: "play.items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Socket_HollowType",
                schema: "play.items",
                table: "Items");

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Elements",
                columns: new[] { "ElementId", "ElementName" },
                values: new object[,]
                {
                    { new Guid("149fa849-be35-40af-b8ae-b1079eb29c52"), "Fire" },
                    { new Guid("19c4d812-4e36-453d-a562-ce618c7bae55"), "Wind" },
                    { new Guid("437da457-5951-4826-bc84-e4c8d3d45ec5"), "Earth" },
                    { new Guid("6794bc09-2099-4fe0-bf8c-32deb9d719bd"), "Water" }
                });
        }
    }
}
