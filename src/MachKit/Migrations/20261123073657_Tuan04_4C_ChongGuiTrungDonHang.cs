using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachKit.Migrations
{
    /// <inheritdoc />
    public partial class Tuan04_4C_ChongGuiTrungDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaYeuCau",
                table: "DonHang",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_DonHang_MaYeuCau",
                table: "DonHang",
                column: "MaYeuCau",
                unique: true,
                filter: "[MaYeuCau] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_DonHang_MaYeuCau",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "MaYeuCau",
                table: "DonHang");
        }
    }
}
