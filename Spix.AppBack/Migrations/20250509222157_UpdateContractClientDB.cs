using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractClientDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractClients",
                columns: table => new
                {
                    ContractClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreado = table.Column<DateTime>(type: "date", nullable: false),
                    ControlContrato = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeCountry = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    CodeNumber = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    EquipoEmpres = table.Column<bool>(type: "bit", nullable: false),
                    EnvoiceClient = table.Column<bool>(type: "bit", nullable: false),
                    ServiceCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractClients", x => x.ContractClientId);
                    table.ForeignKey(
                        name: "FK_ContractClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractClients_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "ContractorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractClients_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractClients_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "ServiceCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractClients_ServiceClients_ServiceClientId",
                        column: x => x.ServiceClientId,
                        principalTable: "ServiceClients",
                        principalColumn: "ServiceClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractClients_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractClients_ClientId",
                table: "ContractClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractClients_ContractorId",
                table: "ContractClients",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractClients_CorporationId_ControlContrato",
                table: "ContractClients",
                columns: new[] { "CorporationId", "ControlContrato" },
                unique: true,
                filter: "[ControlContrato] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContractClients_ServiceCategoryId",
                table: "ContractClients",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractClients_ServiceClientId",
                table: "ContractClients",
                column: "ServiceClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractClients_ZoneId",
                table: "ContractClients",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractClients");
        }
    }
}
