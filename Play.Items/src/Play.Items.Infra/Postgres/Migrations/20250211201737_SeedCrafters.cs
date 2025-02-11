using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class SeedCrafters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("4cf680c0-52d4-4f5b-abda-8a02f8fe920b"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("70882a3c-fd9f-4ad5-8c15-db07584512eb"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("72ab1a67-1807-4b60-b13d-9eafcdb3c09e"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("73b87248-cfb3-46d7-ab9d-b4c137cd949a"));

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Crafters",
                columns: new[] { "CrafterId", "Name" },
                values: new object[,]
                {
                    { new Guid("33364e25-6544-48bd-b87d-37760ee27911"), "Arrgond" },
                    { new Guid("8ce6633f-c318-4017-acef-369b86fd981d"), "Bleatcher" },
                    { new Guid("b69f5ef7-bf93-4de2-a62f-064652d8dd19"), "Din Foo" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Crafters",
                keyColumn: "CrafterId",
                keyValue: new Guid("33364e25-6544-48bd-b87d-37760ee27911"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Crafters",
                keyColumn: "CrafterId",
                keyValue: new Guid("8ce6633f-c318-4017-acef-369b86fd981d"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Crafters",
                keyColumn: "CrafterId",
                keyValue: new Guid("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));

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
                    { new Guid("4cf680c0-52d4-4f5b-abda-8a02f8fe920b"), "Mixing" },
                    { new Guid("70882a3c-fd9f-4ad5-8c15-db07584512eb"), "Weaving" },
                    { new Guid("72ab1a67-1807-4b60-b13d-9eafcdb3c09e"), "Griding" },
                    { new Guid("73b87248-cfb3-46d7-ab9d-b4c137cd949a"), "Forging" }
                });
        }
    }
}
