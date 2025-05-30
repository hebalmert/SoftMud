using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarguesCantToUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Cargues",
                columns: table => new
                {
                    CargueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCargue = table.Column<DateTime>(type: "date", nullable: false),
                    ControlCargue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 20, nullable: false),
                    PurchaseDetailId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 20, nullable: false),
                    CantToUp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargues", x => x.CargueId);
                    table.ForeignKey(
                        name: "FK_Cargues_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cargues_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cargues_PurchaseDetails_PurchaseDetailId",
                        column: x => x.PurchaseDetailId,
                        principalTable: "PurchaseDetails",
                        principalColumn: "PurchaseDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cargues_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CargueDetails",
                columns: table => new
                {
                    CargueDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CargueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MacWlan = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    DateCargue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargueDetails", x => x.CargueDetailId);
                    table.ForeignKey(
                        name: "FK_CargueDetails_Cargues_CargueId",
                        column: x => x.CargueId,
                        principalTable: "Cargues",
                        principalColumn: "CargueId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CargueDetails_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CargueDetails_CargueId",
                table: "CargueDetails",
                column: "CargueId");

            migrationBuilder.CreateIndex(
                name: "IX_CargueDetails_CorporationId",
                table: "CargueDetails",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_CargueDetails_MacWlan_CorporationId",
                table: "CargueDetails",
                columns: new[] { "MacWlan", "CorporationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cargues_CorporationId",
                table: "Cargues",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargues_ProductId",
                table: "Cargues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargues_PurchaseDetailId",
                table: "Cargues",
                column: "PurchaseDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargues_PurchaseId",
                table: "Cargues",
                column: "PurchaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargueDetails");

            migrationBuilder.DropTable(
                name: "Cargues");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
