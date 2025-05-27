using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class TranferSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTransfer = table.Column<DateTime>(type: "date", nullable: false),
                    NroTransfer = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FromStorageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FromProductStorageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToStorageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToProductStorageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferId);
                    table.ForeignKey(
                        name: "FK_Transfers_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferDetails",
                columns: table => new
                {
                    TransferDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameProduct = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferDetails", x => x.TransferDetailsId);
                    table.ForeignKey(
                        name: "FK_TransferDetails_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferDetails_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferDetails_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfers",
                        principalColumn: "TransferId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferDetails_CorporationId_ProductId_TransferId",
                table: "TransferDetails",
                columns: new[] { "CorporationId", "ProductId", "TransferId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferDetails_ProductCategoryId",
                table: "TransferDetails",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferDetails_ProductId",
                table: "TransferDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferDetails_TransferId",
                table: "TransferDetails",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CorporationId_NroTransfer",
                table: "Transfers",
                columns: new[] { "CorporationId", "NroTransfer" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UsuarioId",
                table: "Transfers",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferDetails");

            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
