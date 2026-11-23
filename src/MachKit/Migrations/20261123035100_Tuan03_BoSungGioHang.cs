using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachKit.Migrations
{
    /// <inheritdoc />
    public partial class Tuan03_BoSungGioHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GioHangItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiDungId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    LinhKienId = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    CapNhatUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangItem", x => x.Id);
                    table.CheckConstraint("CK_GioHangItem_SoLuong", "[SoLuong] > 0");
                    table.ForeignKey(
                        name: "FK_GioHangItem_AspNetUsers_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHangItem_LinhKien_LinhKienId",
                        column: x => x.LinhKienId,
                        principalTable: "LinhKien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GioHangItem_LinhKienId",
                table: "GioHangItem",
                column: "LinhKienId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangItem_NguoiDungId_LinhKienId",
                table: "GioHangItem",
                columns: new[] { "NguoiDungId", "LinhKienId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GioHangItem");
        }
    }
}
