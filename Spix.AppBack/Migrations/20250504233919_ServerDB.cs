using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class ServerDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    ServerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IpNetworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    WanName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApiPort = table.Column<int>(type: "int", nullable: false),
                    MarkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarkModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_Servers_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servers_IpNetworks_IpNetworkId",
                        column: x => x.IpNetworkId,
                        principalTable: "IpNetworks",
                        principalColumn: "IpNetworkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servers_MarkModels_MarkModelId",
                        column: x => x.MarkModelId,
                        principalTable: "MarkModels",
                        principalColumn: "MarkModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servers_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "MarkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servers_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servers_CorporationId",
                table: "Servers",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_IpNetworkId_CorporationId",
                table: "Servers",
                columns: new[] { "IpNetworkId", "CorporationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_MarkId",
                table: "Servers",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_MarkModelId",
                table: "Servers",
                column: "MarkModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_ServerName_CorporationId",
                table: "Servers",
                columns: new[] { "ServerName", "CorporationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_ZoneId",
                table: "Servers",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
