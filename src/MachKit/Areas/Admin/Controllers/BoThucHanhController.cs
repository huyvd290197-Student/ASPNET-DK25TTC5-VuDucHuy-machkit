using MachKit.Data;
using MachKit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class BoThucHanhController : Controller
{
    private readonly AppDbContext _db;
    public BoThucHanhController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.BoThucHanhs.Include(x => x.ThanhPhans).OrderBy(x => x.MaBo).ToListAsync());

    public IActionResult Create() => View(new BoThucHanh());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BoThucHanh model)
    {
        if (await _db.BoThucHanhs.AnyAsync(x => x.MaBo == model.MaBo)) ModelState.AddModelError(nameof(model.MaBo), "Mã bộ đã tồn tại.");
        if (!ModelState.IsValid) return View(model);
        _db.Add(model); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _db.BoThucHanhs.FindAsync(id);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BoThucHanh model)
    {
        if (id != model.Id) return NotFound();
        if (await _db.BoThucHanhs.AnyAsync(x => x.MaBo == model.MaBo && x.Id != id)) ModelState.AddModelError(nameof(model.MaBo), "Mã bộ đã tồn tại.");
        if (!ModelState.IsValid) return View(model);
        _db.Update(model); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ThanhPhan(int id)
    {
        var bo = await _db.BoThucHanhs.Include(x => x.ThanhPhans).ThenInclude(x => x.LinhKien).FirstOrDefaultAsync(x => x.Id == id);
        if (bo is null) return NotFound();
        ViewBag.LinhKienId = new SelectList(await _db.LinhKiens.Where(x => !x.NgungBan).OrderBy(x => x.TenLinhKien).ToListAsync(), "Id", "TenLinhKien");
        return View(bo);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ThemThanhPhan(int boId, int linhKienId, int soLuong)
    {
        if (soLuong < 1) soLuong = 1;
        var row = await _db.ThanhPhanBos.FindAsync(boId, linhKienId);
        if (row is null) _db.ThanhPhanBos.Add(new ThanhPhanBo { BoThucHanhId = boId, LinhKienId = linhKienId, SoLuong = soLuong });
        else row.SoLuong = soLuong;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(ThanhPhan), new { id = boId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> XoaThanhPhan(int boId, int linhKienId)
    {
        var row = await _db.ThanhPhanBos.FindAsync(boId, linhKienId);
        if (row is not null) { _db.Remove(row); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(ThanhPhan), new { id = boId });
    }
}
