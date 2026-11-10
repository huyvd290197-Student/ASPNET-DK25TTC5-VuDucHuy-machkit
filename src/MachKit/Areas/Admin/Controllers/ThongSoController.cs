using MachKit.Data;
using MachKit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class ThongSoController : Controller
{
    private readonly AppDbContext _db;
    public ThongSoController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(int linhKienId)
    {
        var lk = await _db.LinhKiens.FindAsync(linhKienId);
        if (lk is null) return NotFound();
        ViewBag.LinhKien = lk;
        return View(await _db.ThongSoKyThuats.Where(x => x.LinhKienId == linhKienId).ToListAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int linhKienId, string tenThongSo, string giaTri, string? donVi)
    {
        if (string.IsNullOrWhiteSpace(tenThongSo) || string.IsNullOrWhiteSpace(giaTri)) return RedirectToAction(nameof(Index), new { linhKienId });
        _db.ThongSoKyThuats.Add(new ThongSoKyThuat { LinhKienId = linhKienId, TenThongSo = tenThongSo.Trim(), GiaTri = giaTri.Trim(), DonVi = donVi?.Trim() });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { linhKienId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ts = await _db.ThongSoKyThuats.FindAsync(id);
        if (ts is null) return NotFound();
        int lk = ts.LinhKienId;
        _db.Remove(ts); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { linhKienId = lk });
    }
}
