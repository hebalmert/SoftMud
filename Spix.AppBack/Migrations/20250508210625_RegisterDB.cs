using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class RegisterDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    RegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Contratos = table.Column<int>(type: "int", nullable: false),
                    Solicitudes = table.Column<int>(type: "int", nullable: false),
                    Cargue = table.Column<int>(type: "int", nullable: false),
                    Egresos = table.Column<int>(type: "int", nullable: false),
                    Adelantado = table.Column<int>(type: "int", nullable: false),
                    Exonerado = table.Column<int>(type: "int", nullable: false),
                    NotaCobro = table.Column<int>(type: "int", nullable: false),
                    Factura = table.Column<int>(type: "int", nullable: false),
                    PagoContratista = table.Column<int>(type: "int", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.RegisterId);
                    table.ForeignKey(
                        name: "FK_Registers_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Adelantado",
                table: "Registers",
                columns: new[] { "CorporationId", "Adelantado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Cargue",
                table: "Registers",
                columns: new[] { "CorporationId", "Cargue" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Contratos",
                table: "Registers",
                columns: new[] { "CorporationId", "Contratos" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Egresos",
                table: "Registers",
                columns: new[] { "CorporationId", "Egresos" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Exonerado",
                table: "Registers",
                columns: new[] { "CorporationId", "Exonerado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Factura",
                table: "Registers",
                columns: new[] { "CorporationId", "Factura" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_NotaCobro",
                table: "Registers",
                columns: new[] { "CorporationId", "NotaCobro" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_PagoContratista",
                table: "Registers",
                columns: new[] { "CorporationId", "PagoContratista" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporationId_Solicitudes",
                table: "Registers",
                columns: new[] { "CorporationId", "Solicitudes" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registers");
        }
    }
}
