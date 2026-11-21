using MachKit.Data;
using MachKit.Models;
using MachKit.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Controllers;

// Controller phía khách: KHÔNG có [Area("Admin")].
public class LinhKienController : Controller
{
    private readonly AppDbContext _db;
    private const int KichThuocTrang = 6;

    public LinhKienController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? tuKhoa,
        int? danhMucId,
        int? thuongHieuId,
        decimal? giaTu,
        decimal? giaDen,
        string? sapXep,
        int trang = 1)
    {
        // Chuẩn hóa giá trị từ URL; không để chuỗi tìm kiếm quá dài.
        tuKhoa = tuKhoa?.Trim();
        if (tuKhoa?.Length > 100)
            tuKhoa = tuKhoa[..100];

        if (giaTu < 0) giaTu = null;
        if (giaDen < 0) giaDen = null;

        if (giaTu.HasValue && giaDen.HasValue && giaTu > giaDen)
        {
            ModelState.AddModelError(
                string.Empty,
                "Giá từ phải nhỏ hơn hoặc bằng giá đến.");
        }

        // Chỉ chấp nhận các cách sắp xếp đã quy định.
        sapXep = sapXep switch
        {
            "gia_tang" => "gia_tang",
            "gia_giam" => "gia_giam",
            "ten_za" => "ten_za",
            _ => "ten_az"
        };

        // Đây là truy vấn EF Core (SQL Server), chưa tải mọi dòng về RAM.
        IQueryable<LinhKien> truyVan = _db.LinhKiens
            .AsNoTracking()
            .Where(x => !x.NgungBan);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            truyVan = truyVan.Where(x =>
                x.TenLinhKien.Contains(tuKhoa) ||
                x.MaLinhKien.Contains(tuKhoa));
        }

        if (danhMucId.HasValue && danhMucId > 0)
            truyVan = truyVan.Where(x => x.DanhMucId == danhMucId.Value);

        if (thuongHieuId.HasValue && thuongHieuId > 0)
            truyVan = truyVan.Where(x => x.ThuongHieuId == thuongHieuId.Value);

        if (giaTu.HasValue)
            truyVan = truyVan.Where(x => x.Gia >= giaTu.Value);

        if (giaDen.HasValue)
            truyVan = truyVan.Where(x => x.Gia <= giaDen.Value);

        // Đếm sau khi lọc nhưng trước khi phân trang.
        int tongSo = await truyVan.CountAsync();
        int tongTrang = (int)Math.Ceiling(tongSo / (double)KichThuocTrang);

        if (trang < 1) trang = 1;
        if (tongTrang > 0 && trang > tongTrang) trang = tongTrang;
        if (tongTrang == 0) trang = 1;

        // Thêm Id làm khóa phụ để việc phân trang có thứ tự ổn định.
        IOrderedQueryable<LinhKien> daSapXep = sapXep switch
        {
            "gia_tang" => truyVan.OrderBy(x => x.Gia).ThenBy(x => x.Id),
            "gia_giam" => truyVan.OrderByDescending(x => x.Gia).ThenBy(x => x.Id),
            "ten_za" => truyVan.OrderByDescending(x => x.TenLinhKien).ThenBy(x => x.Id),
            _ => truyVan.OrderBy(x => x.TenLinhKien).ThenBy(x => x.Id)
        };

        var linhKiens = await daSapXep
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Skip((trang - 1) * KichThuocTrang)
            .Take(KichThuocTrang)
            .ToListAsync();

        var model = new LinhKienDanhSachViewModel
        {
            TuKhoa = tuKhoa,
            DanhMucId = danhMucId,
            ThuongHieuId = thuongHieuId,
            GiaTu = giaTu,
            GiaDen = giaDen,
            SapXep = sapXep,
            Trang = trang,
            KichThuocTrang = KichThuocTrang,
            TongSo = tongSo,
            LinhKiens = linhKiens,
            DanhMucs = await _db.DanhMucs
                .AsNoTracking()
                .OrderBy(x => x.ThuTu)
                .ThenBy(x => x.TenDanhMuc)
                .ToListAsync(),
            ThuongHieus = await _db.ThuongHieus
                .AsNoTracking()
                .OrderBy(x => x.TenThuongHieu)
                .ToListAsync()
        };

        return View(model);
    }
}
