using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesDataDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChainTypes",
                columns: table => new
                {
                    ChainTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChainName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChainTypes", x => x.ChainTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "FrecuencyTypes",
                columns: table => new
                {
                    FrecuencyTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrecuencyTypes", x => x.FrecuencyTypeId);
                });

            migrationBuilder.CreateTable(
                name: "HotSpotTypes",
                columns: table => new
                {
                    HotSpotTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotSpotTypes", x => x.HotSpotTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    OperationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.OperationId);
                });

            migrationBuilder.CreateTable(
                name: "Securities",
                columns: table => new
                {
                    SecurityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Securities", x => x.SecurityId);
                });

            migrationBuilder.CreateTable(
                name: "Frecuencies",
                columns: table => new
                {
                    FrecuencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrecuencyTypeId = table.Column<int>(type: "int", nullable: false),
                    FrecuencyName = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frecuencies", x => x.FrecuencyId);
                    table.ForeignKey(
                        name: "FK_Frecuencies_FrecuencyTypes_FrecuencyTypeId",
                        column: x => x.FrecuencyTypeId,
                        principalTable: "FrecuencyTypes",
                        principalColumn: "FrecuencyTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChainTypes_ChainName",
                table: "ChainTypes",
                column: "ChainName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ChannelName",
                table: "Channels",
                column: "ChannelName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Frecuencies_FrecuencyTypeId_FrecuencyName",
                table: "Frecuencies",
                columns: new[] { "FrecuencyTypeId", "FrecuencyName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FrecuencyTypes_TypeName",
                table: "FrecuencyTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotSpotTypes_TypeName",
                table: "HotSpotTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_OperationName",
                table: "Operations",
                column: "OperationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Securities_SecurityName",
                table: "Securities",
                column: "SecurityName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChainTypes");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Frecuencies");

            migrationBuilder.DropTable(
                name: "HotSpotTypes");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Securities");

            migrationBuilder.DropTable(
                name: "FrecuencyTypes");
        }
    }
}
