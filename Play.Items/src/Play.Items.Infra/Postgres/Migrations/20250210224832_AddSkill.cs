using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "play.items",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "CrafterSkill",
                schema: "play.items",
                columns: table => new
                {
                    CrafterId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillsSkillId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrafterSkill", x => new { x.CrafterId, x.SkillsSkillId });
                    table.ForeignKey(
                        name: "FK_CrafterSkill_Crafters_CrafterId",
                        column: x => x.CrafterId,
                        principalSchema: "play.items",
                        principalTable: "Crafters",
                        principalColumn: "CrafterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrafterSkill_Skills_SkillsSkillId",
                        column: x => x.SkillsSkillId,
                        principalSchema: "play.items",
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrafterSkill_SkillsSkillId",
                schema: "play.items",
                table: "CrafterSkill",
                column: "SkillsSkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrafterSkill",
                schema: "play.items");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "play.items");
        }
    }
}
