using MachKit.Data;
using MachKit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class ThuongHieuController : Controller
{
    private readonly AppDbContext _db;
    public ThuongHieuController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.ThuongHieus.OrderBy(x => x.TenThuongHieu).ToListAsync());

    public IActionResult Create() => View(new ThuongHieu());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ThuongHieu model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Add(model); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _db.ThuongHieus.FindAsync(id);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ThuongHieu model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(model);
        _db.Update(model); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
