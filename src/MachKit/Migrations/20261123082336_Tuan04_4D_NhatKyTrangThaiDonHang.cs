using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachKit.Migrations
{
    /// <inheritdoc />
    public partial class Tuan04_4D_NhatKyTrangThaiDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LichSuTrangThaiDonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonHangId = table.Column<int>(type: "int", nullable: false),
                    MaThaoTac = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NguoiThucHienId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TrangThaiCu = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TrangThaiMoi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThoiDiemUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaHoanTon = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuTrangThaiDonHang", x => x.Id);
                    table.CheckConstraint("CK_LichSuDonHang_HoanTon", "([TrangThaiMoi] = N'DaHuy' AND [DaHoanTon] = 1) OR ([TrangThaiMoi] <> N'DaHuy' AND [DaHoanTon] = 0)");
                    table.ForeignKey(
                        name: "FK_LichSuTrangThaiDonHang_AspNetUsers_NguoiThucHienId",
                        column: x => x.NguoiThucHienId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuTrangThaiDonHang_DonHang_DonHangId",
                        column: x => x.DonHangId,
                        principalTable: "DonHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTrangThaiDonHang_DonHangId_ThoiDiemUtc_Id",
                table: "LichSuTrangThaiDonHang",
                columns: new[] { "DonHangId", "ThoiDiemUtc", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTrangThaiDonHang_NguoiThucHienId",
                table: "LichSuTrangThaiDonHang",
                column: "NguoiThucHienId");

            migrationBuilder.CreateIndex(
                name: "UX_LichSuDonHang_HuyMotLan",
                table: "LichSuTrangThaiDonHang",
                column: "DonHangId",
                unique: true,
                filter: "[TrangThaiMoi] = N'DaHuy'");

            migrationBuilder.CreateIndex(
                name: "UX_LichSuDonHang_MaThaoTac",
                table: "LichSuTrangThaiDonHang",
                column: "MaThaoTac",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichSuTrangThaiDonHang");
        }
    }
}
