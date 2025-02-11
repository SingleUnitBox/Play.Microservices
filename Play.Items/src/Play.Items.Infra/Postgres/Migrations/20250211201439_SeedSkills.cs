using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class SeedSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
