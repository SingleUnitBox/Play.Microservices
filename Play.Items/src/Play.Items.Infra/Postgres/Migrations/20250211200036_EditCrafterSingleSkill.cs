using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Items.Infra.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EditCrafterSingleSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrafterSkill",
                schema: "play.items");

            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                schema: "play.items",
                table: "Crafters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Crafters_SkillId",
                schema: "play.items",
                table: "Crafters",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crafters_Skills_SkillId",
                schema: "play.items",
                table: "Crafters",
                column: "SkillId",
                principalSchema: "play.items",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crafters_Skills_SkillId",
                schema: "play.items",
                table: "Crafters");

            migrationBuilder.DropIndex(
                name: "IX_Crafters_SkillId",
                schema: "play.items",
                table: "Crafters");

            migrationBuilder.DropColumn(
                name: "SkillId",
                schema: "play.items",
                table: "Crafters");

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
    }
}
