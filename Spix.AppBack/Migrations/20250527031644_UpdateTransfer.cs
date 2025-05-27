using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Usuarios_UsuarioId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_UsuarioId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Transfers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserId",
                table: "Transfers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AspNetUsers_UserId",
                table: "Transfers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AspNetUsers_UserId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_UserId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transfers");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UsuarioId",
                table: "Transfers",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Usuarios_UsuarioId",
                table: "Transfers",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
