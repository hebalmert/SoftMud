using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class IpNetIpNetworkDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IpNets",
                columns: table => new
                {
                    IpNetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Assigned = table.Column<bool>(type: "bit", nullable: false),
                    Excluded = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpNets", x => x.IpNetId);
                    table.ForeignKey(
                        name: "FK_IpNets_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IpNetworks",
                columns: table => new
                {
                    IpNetworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Assigned = table.Column<bool>(type: "bit", nullable: false),
                    Excluded = table.Column<bool>(type: "bit", nullable: false),
                    CorporationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpNetworks", x => x.IpNetworkId);
                    table.ForeignKey(
                        name: "FK_IpNetworks_Corporations_CorporationId",
                        column: x => x.CorporationId,
                        principalTable: "Corporations",
                        principalColumn: "CorporationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IpNets_CorporationId",
                table: "IpNets",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_IpNets_Ip_CorporationId",
                table: "IpNets",
                columns: new[] { "Ip", "CorporationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IpNetworks_CorporationId",
                table: "IpNetworks",
                column: "CorporationId");

            migrationBuilder.CreateIndex(
                name: "IX_IpNetworks_Ip_CorporationId",
                table: "IpNetworks",
                columns: new[] { "Ip", "CorporationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpNets");

            migrationBuilder.DropTable(
                name: "IpNetworks");
        }
    }
}
