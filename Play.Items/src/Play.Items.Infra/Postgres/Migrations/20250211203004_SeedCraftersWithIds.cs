using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class SeedCraftersWithIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("9366cd4d-1615-47cb-ba90-695c46814a83"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("c12b527c-8b5b-4344-94b0-76e65ccc6a78"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("e5f4a62c-8f49-49d3-b9a4-9e89acd685fa"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("e9c81cb6-810a-439c-bb9f-aaea0b4bcd77"));

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Skills",
                columns: new[] { "SkillId", "SkillName" },
                values: new object[,]
                {
                    { new Guid("00488f14-e7c9-4044-9388-8a231ee8d5d8"), "Mixing" },
                    { new Guid("4e3ab89c-944c-4dea-ab47-2a4c2d88766b"), "Forging" },
                    { new Guid("e44a27bd-f845-4d91-b052-af025412f947"), "Weaving" },
                    { new Guid("edf68ffe-e9bf-4a44-87a4-64362c3753f6"), "Griding" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("00488f14-e7c9-4044-9388-8a231ee8d5d8"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("4e3ab89c-944c-4dea-ab47-2a4c2d88766b"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("e44a27bd-f845-4d91-b052-af025412f947"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("edf68ffe-e9bf-4a44-87a4-64362c3753f6"));

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Skills",
                columns: new[] { "SkillId", "SkillName" },
                values: new object[,]
                {
                    { new Guid("9366cd4d-1615-47cb-ba90-695c46814a83"), "Forging" },
                    { new Guid("c12b527c-8b5b-4344-94b0-76e65ccc6a78"), "Mixing" },
                    { new Guid("e5f4a62c-8f49-49d3-b9a4-9e89acd685fa"), "Weaving" },
                    { new Guid("e9c81cb6-810a-439c-bb9f-aaea0b4bcd77"), "Griding" }
                });
        }
    }
}
