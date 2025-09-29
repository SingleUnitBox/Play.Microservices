﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Common.Messaging.Deduplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeduplicationEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "deduplication");

            migrationBuilder.CreateTable(
                name: "DeduplicationEntries",
                schema: "deduplication",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "text", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeduplicationEntries", x => x.MessageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeduplicationEntries",
                schema: "deduplication");
        }
    }
}
