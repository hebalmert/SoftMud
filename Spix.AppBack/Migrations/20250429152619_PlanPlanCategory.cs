using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class PlanPlanCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanCategories",
                columns: table => new
                {
                    PlanCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanCategories", x => x.PlanCategoryId);
                    table.ForeignKey(
                        name: "FK_PlanCategories_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpeedUp = table.Column<int>(type: "int", nullable: false),
                    SpeedUpType = table.Column<int>(type: "int", nullable: false),
                    SpeedDown = table.Column<int>(type: "int", nullable: false),
                    SpeedDownType = table.Column<int>(type: "int", nullable: false),
                    TasaReuso = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                    table.ForeignKey(
                        name: "FK_Plans_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plans_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "PlanCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanCategories_CorporationId",
                table: "PlanCategories",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanCategories_PlanCategoryName_CorporationId",
                table: "PlanCategories",
                columns: new[] { "PlanCategoryName", "CorporationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_CorporationId_PlanName",
                table: "Plans",
                columns: new[] { "CorporationId", "PlanName" });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanCategoryId",
                table: "Plans",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanId",
                table: "Plans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_TaxId",
                table: "Plans",
                column: "TaxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "PlanCategories");
        }
    }
}
