using System.Globalization;
using MachKit.Data;
using MachKit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Controllers;

public sealed class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index(string? soBo)
    {
        string nhap = Request.Query.ContainsKey("soBo") ? (soBo?.Trim() ?? "") : "1";
        bool hopLe = int.TryParse(nhap, NumberStyles.None, CultureInfo.InvariantCulture, out int n)
                     && n >= 1 && n <= 99;

        var bo = await _db.BoThucHanhs
            .AsNoTracking()
            .Include(x => x.ThanhPhans)
                .ThenInclude(x => x.LinhKien)
            .FirstOrDefaultAsync(x => x.KichHoat);

        if (bo is null || bo.ThanhPhans.Count == 0)
        {
            return View(new DuToanBo
            {
                GiaTriNhap = nhap,
                Loi = "Chưa có dữ liệu bộ thực hành trong cơ sở dữ liệu.",
                Dong = Array.Empty<DongDuToan>(),
                SoBoToiDa = 0
            });
        }

        var cauThanh = bo.ThanhPhans
            .Where(x => x.LinhKien is not null && !x.LinhKien.NgungBan)
            .Select(x => new LinhKienMau(
                x.LinhKien!.MaLinhKien,
                x.LinhKien.TenLinhKien,
                x.SoLuong,
                x.LinhKien.SoLuongTon,
                x.LinhKien.Gia))
            .ToArray();

        int soBoToiDa = cauThanh.Length == 0 ? 0 : cauThanh.Min(x => x.Ton / x.DinhMuc);

        return View(new DuToanBo
        {
            GiaTriNhap = nhap,
            Loi = hopLe ? null : "Hãy nhập số bộ là số nguyên từ 1 đến 99.",
            Dong = hopLe ? cauThanh.Select(x => new DongDuToan(x, n)).ToArray() : Array.Empty<DongDuToan>(),
            SoBoToiDa = soBoToiDa
        });
    }
}
