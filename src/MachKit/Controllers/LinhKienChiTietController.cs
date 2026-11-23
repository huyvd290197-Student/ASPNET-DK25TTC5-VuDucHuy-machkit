using MachKit.Data;
using MachKit.Helpers;
using MachKit.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Controllers;

// Controller riêng: không thay thế LinhKienController của Chặng 3A.
[AllowAnonymous]
public class LinhKienChiTietController : Controller
{
    private readonly AppDbContext _db;

    public LinhKienChiTietController(AppDbContext db) => _db = db;

    [HttpGet("LinhKien/ChiTiet/{slug}")]
    public async Task<IActionResult> Index(string slug, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(slug) || slug.Length > 220) return NotFound();

        var sanPham = await _db.LinhKiens.AsNoTracking()
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.ThongSos)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => !x.NgungBan && x.Slug == slug, cancellationToken);

        if (sanPham is null) return NotFound();

        var lienQuan = await _db.LinhKiens.AsNoTracking()
            .Where(x => !x.NgungBan && x.Id != sanPham.Id
                && x.DanhMucId == sanPham.DanhMucId)
            .OrderBy(x => x.TenLinhKien).ThenBy(x => x.Id)
            .Take(4).ToListAsync(cancellationToken);

        return View(new LinhKienChiTietViewModel
        {
            SanPham = sanPham,
            SanPhamLienQuan = lienQuan,
            DatasheetHttpUrl = Tuan03HienThiHelper.DatasheetHttp(sanPham.DatasheetUrl)
        });
    }
}
