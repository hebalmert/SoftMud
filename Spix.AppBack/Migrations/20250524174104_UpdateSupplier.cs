using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spix.AppBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_CorporationId_NroDocument",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "NroDocument",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "CodeCountry",
                table: "Suppliers",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodeNumber",
                table: "Suppliers",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "Suppliers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Suppliers",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Suppliers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CorporationId_Document_DocumentTypeId",
                table: "Suppliers",
                columns: new[] { "CorporationId", "Document", "DocumentTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_DocumentTypeId",
                table: "Suppliers",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_DocumentTypes_DocumentTypeId",
                table: "Suppliers",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "DocumentTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_DocumentTypes_DocumentTypeId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_CorporationId_Document_DocumentTypeId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_DocumentTypeId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CodeCountry",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CodeNumber",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NroDocument",
                table: "Suppliers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CorporationId_NroDocument",
                table: "Suppliers",
                columns: new[] { "CorporationId", "NroDocument" },
                unique: true);
        }
    }
}
