using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class ComplementodContractIPHastaQues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractIps",
                columns: table => new
                {
                    ContractIpId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpNetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractIps", x => x.ContractIpId);
                    table.ForeignKey(
                        name: "FK_ContractIps_ContractClients_ContractClientId",
                        column: x => x.ContractClientId,
                        principalTable: "ContractClients",
                        principalColumn: "ContractClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractIps_IpNets_IpNetId",
                        column: x => x.IpNetId,
                        principalTable: "IpNets",
                        principalColumn: "IpNetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractNodes",
                columns: table => new
                {
                    ContractNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractNodes", x => x.ContractNodeId);
                    table.ForeignKey(
                        name: "FK_ContractNodes_ContractClients_ContractClientId",
                        column: x => x.ContractClientId,
                        principalTable: "ContractClients",
                        principalColumn: "ContractClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractNodes_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractPlans",
                columns: table => new
                {
                    ContractPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractPlans", x => x.ContractPlanId);
                    table.ForeignKey(
                        name: "FK_ContractPlans_ContractClients_ContractClientId",
                        column: x => x.ContractClientId,
                        principalTable: "ContractClients",
                        principalColumn: "ContractClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractPlans_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractQues",
                columns: table => new
                {
                    ContractQueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpNetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpServer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalVelocidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MikrotikId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractQues", x => x.ContractQueId);
                    table.ForeignKey(
                        name: "FK_ContractQues_ContractClients_ContractClientId",
                        column: x => x.ContractClientId,
                        principalTable: "ContractClients",
                        principalColumn: "ContractClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractQues_IpNets_IpNetId",
                        column: x => x.IpNetId,
                        principalTable: "IpNets",
                        principalColumn: "IpNetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractQues_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractQues_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractServers",
                columns: table => new
                {
                    ContractServerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractServers", x => x.ContractServerId);
                    table.ForeignKey(
                        name: "FK_ContractServers_ContractClients_ContractClientId",
                        column: x => x.ContractClientId,
                        principalTable: "ContractClients",
                        principalColumn: "ContractClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractServers_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractIps_ContractClientId_IpNetId",
                table: "ContractIps",
                columns: new[] { "ContractClientId", "IpNetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractIps_IpNetId",
                table: "ContractIps",
                column: "IpNetId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractNodes_ContractClientId_NodeId",
                table: "ContractNodes",
                columns: new[] { "ContractClientId", "NodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractNodes_NodeId",
                table: "ContractNodes",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractPlans_ContractClientId_PlanId",
                table: "ContractPlans",
                columns: new[] { "ContractClientId", "PlanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractPlans_PlanId",
                table: "ContractPlans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractQues_ContractClientId_IpNetId",
                table: "ContractQues",
                columns: new[] { "ContractClientId", "IpNetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractQues_IpNetId",
                table: "ContractQues",
                column: "IpNetId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractQues_PlanId",
                table: "ContractQues",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractQues_ServerId",
                table: "ContractQues",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServers_ContractClientId_ServerId",
                table: "ContractServers",
                columns: new[] { "ContractClientId", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractServers_ServerId",
                table: "ContractServers",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractIps");

            migrationBuilder.DropTable(
                name: "ContractNodes");

            migrationBuilder.DropTable(
                name: "ContractPlans");

            migrationBuilder.DropTable(
                name: "ContractQues");

            migrationBuilder.DropTable(
                name: "ContractServers");
        }
    }
}
