using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class NodesDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodesName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IpNetworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    Mac = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    MarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarkModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    APClientes = table.Column<bool>(type: "bit", nullable: false),
                    FrecuencyTypeId = table.Column<int>(type: "int", nullable: true),
                    FrecuencyId = table.Column<int>(type: "int", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: true),
                    SecurityId = table.Column<int>(type: "int", nullable: true),
                    FraseSeguridad = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.NodeId);
                    table.ForeignKey(
                        name: "FK_Nodes_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nodes_Frecuencies_FrecuencyId",
                        column: x => x.FrecuencyId,
                        principalTable: "Frecuencies",
                        principalColumn: "FrecuencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_FrecuencyTypes_FrecuencyTypeId",
                        column: x => x.FrecuencyTypeId,
                        principalTable: "FrecuencyTypes",
                        principalColumn: "FrecuencyTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_IpNetworks_IpNetworkId",
                        column: x => x.IpNetworkId,
                        principalTable: "IpNetworks",
                        principalColumn: "IpNetworkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_MarkModels_MarkModelId",
                        column: x => x.MarkModelId,
                        principalTable: "MarkModels",
                        principalColumn: "MarkModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "MarkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "OperationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_Securities_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "Securities",
                        principalColumn: "SecurityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nodes_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ChannelId",
                table: "Nodes",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_CorporationId",
                table: "Nodes",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_FrecuencyId",
                table: "Nodes",
                column: "FrecuencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_FrecuencyTypeId",
                table: "Nodes",
                column: "FrecuencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_IpNetworkId",
                table: "Nodes",
                column: "IpNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_MarkId",
                table: "Nodes",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_MarkModelId",
                table: "Nodes",
                column: "MarkModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_NodesName_CorporationId_OperationId",
                table: "Nodes",
                columns: new[] { "NodesName", "CorporationId", "OperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_OperationId",
                table: "Nodes",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_SecurityId",
                table: "Nodes",
                column: "SecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ZoneId",
                table: "Nodes",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}
