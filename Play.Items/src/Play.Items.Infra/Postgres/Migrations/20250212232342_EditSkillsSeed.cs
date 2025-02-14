using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EditSkillsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("4b392707-748b-4f62-9545-e93feb1827cf"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("5eb6e794-28a0-47ad-bec8-d4ce7612f7c2"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("e9aed6b8-92b6-4ffe-8dcd-25287e553b4d"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Elements",
                keyColumn: "ElementId",
                keyValue: new Guid("fc10a749-c4ae-4f6d-8966-4a51a1ac1af3"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("848baf22-f924-47a1-a300-a33b5f71d9e4"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("ac63eefa-1aa0-42a2-bf77-8ab3595875ab"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("c8d0f3a9-3d45-4742-b54d-f2b245abbde6"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("cab5a2f1-8fc3-4d1c-9a8c-ef4928779481"));

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

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Skills",
                columns: new[] { "SkillId", "SkillName" },
                values: new object[,]
                {
                    { new Guid("4b392707-748b-4f62-9545-e93feb1827cf"), "Forging" },
                    { new Guid("5eb6e794-28a0-47ad-bec8-d4ce7612f7c2"), "Mixing" },
                    { new Guid("e9aed6b8-92b6-4ffe-8dcd-25287e553b4d"), "Weaving" },
                    { new Guid("fc10a749-c4ae-4f6d-8966-4a51a1ac1af3"), "Griding" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("4b392707-748b-4f62-9545-e93feb1827cf"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("5eb6e794-28a0-47ad-bec8-d4ce7612f7c2"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("e9aed6b8-92b6-4ffe-8dcd-25287e553b4d"));

            migrationBuilder.DeleteData(
                schema: "play.items",
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: new Guid("fc10a749-c4ae-4f6d-8966-4a51a1ac1af3"));

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Elements",
                columns: new[] { "ElementId", "ElementName" },
                values: new object[,]
                {
                    { new Guid("4b392707-748b-4f62-9545-e93feb1827cf"), "Earth" },
                    { new Guid("5eb6e794-28a0-47ad-bec8-d4ce7612f7c2"), "Water" },
                    { new Guid("e9aed6b8-92b6-4ffe-8dcd-25287e553b4d"), "Wind" },
                    { new Guid("fc10a749-c4ae-4f6d-8966-4a51a1ac1af3"), "Fire" }
                });

            migrationBuilder.InsertData(
                schema: "play.items",
                table: "Skills",
                columns: new[] { "SkillId", "SkillName" },
                values: new object[,]
                {
                    { new Guid("848baf22-f924-47a1-a300-a33b5f71d9e4"), "Forging" },
                    { new Guid("ac63eefa-1aa0-42a2-bf77-8ab3595875ab"), "Weaving" },
                    { new Guid("c8d0f3a9-3d45-4742-b54d-f2b245abbde6"), "Griding" },
                    { new Guid("cab5a2f1-8fc3-4d1c-9a8c-ef4928779481"), "Mixing" }
                });
        }
    }
}
