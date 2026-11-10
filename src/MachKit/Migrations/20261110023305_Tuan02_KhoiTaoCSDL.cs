using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachKit.Migrations
{
    /// <inheritdoc />
    public partial class Tuan02_KhoiTaoCSDL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoThucHanh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TenBo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KichHoat = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoThucHanh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMuc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMuc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuocGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinhKien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLinhKien = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TenLinhKien = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(18,0)", precision: 18, scale: 0, nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    MucTonToiThieu = table.Column<int>(type: "int", nullable: false),
                    DanhMucId = table.Column<int>(type: "int", nullable: false),
                    ThuongHieuId = table.Column<int>(type: "int", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatasheetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgungBan = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhKien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinhKien_DanhMuc_DanhMucId",
                        column: x => x.DanhMucId,
                        principalTable: "DanhMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinhKien_ThuongHieu_ThuongHieuId",
                        column: x => x.ThuongHieuId,
                        principalTable: "ThuongHieu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThanhPhanBo",
                columns: table => new
                {
                    BoThucHanhId = table.Column<int>(type: "int", nullable: false),
                    LinhKienId = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhPhanBo", x => new { x.BoThucHanhId, x.LinhKienId });
                    table.ForeignKey(
                        name: "FK_ThanhPhanBo_BoThucHanh_BoThucHanhId",
                        column: x => x.BoThucHanhId,
                        principalTable: "BoThucHanh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThanhPhanBo_LinhKien_LinhKienId",
                        column: x => x.LinhKienId,
                        principalTable: "LinhKien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThongSoKyThuat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinhKienId = table.Column<int>(type: "int", nullable: false),
                    TenThongSo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaTri = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DonVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongSoKyThuat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongSoKyThuat_LinhKien_LinhKienId",
                        column: x => x.LinhKienId,
                        principalTable: "LinhKien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoThucHanh_MaBo",
                table: "BoThucHanh",
                column: "MaBo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinhKien_DanhMucId",
                table: "LinhKien",
                column: "DanhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhKien_MaLinhKien",
                table: "LinhKien",
                column: "MaLinhKien",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinhKien_ThuongHieuId",
                table: "LinhKien",
                column: "ThuongHieuId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhPhanBo_LinhKienId",
                table: "ThanhPhanBo",
                column: "LinhKienId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongSoKyThuat_LinhKienId",
                table: "ThongSoKyThuat",
                column: "LinhKienId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThanhPhanBo");

            migrationBuilder.DropTable(
                name: "ThongSoKyThuat");

            migrationBuilder.DropTable(
                name: "BoThucHanh");

            migrationBuilder.DropTable(
                name: "LinhKien");

            migrationBuilder.DropTable(
                name: "DanhMuc");

            migrationBuilder.DropTable(
                name: "ThuongHieu");
        }
    }
}
