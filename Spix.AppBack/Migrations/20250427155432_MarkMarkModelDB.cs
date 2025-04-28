using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class MarkMarkModelDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    MarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarkName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.MarkId);
                    table.ForeignKey(
                        name: "FK_Marks_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkModels",
                columns: table => new
                {
                    MarkModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarkModelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkModels", x => x.MarkModelId);
                    table.ForeignKey(
                        name: "FK_MarkModels_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarkModels_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "MarkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarkModels_CorporationId",
                table: "MarkModels",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkModels_MarkId",
                table: "MarkModels",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkModels_MarkModelName_CorporationId_MarkId",
                table: "MarkModels",
                columns: new[] { "MarkModelName", "CorporationId", "MarkId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_CorporationId",
                table: "Marks",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_MarkName_CorporationId",
                table: "Marks",
                columns: new[] { "MarkName", "CorporationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkModels");

            migrationBuilder.DropTable(
                name: "Marks");
        }
    }
}
