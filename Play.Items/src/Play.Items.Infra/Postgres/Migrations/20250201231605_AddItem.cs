using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "play.items");

            migrationBuilder.CreateTable(
                name: "Crafters",
                schema: "play.items",
                columns: table => new
                {
                    CrafterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crafters", x => x.CrafterId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "play.items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CrafterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Crafters_CrafterId",
                        column: x => x.CrafterId,
                        principalSchema: "play.items",
                        principalTable: "Crafters",
                        principalColumn: "CrafterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CrafterId",
                schema: "play.items",
                table: "Items",
                column: "CrafterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items",
                schema: "play.items");

            migrationBuilder.DropTable(
                name: "Crafters",
                schema: "play.items");
        }
    }
}
