using MachKit.Data;
using MachKit.Helpers;
using MachKit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class DanhMucController : Controller
{
    private readonly AppDbContext _db;

    public DanhMucController(AppDbContext db)
    {
        _db = db;
    }

    // DANH SÁCH
    public async Task<IActionResult> Index()
    {
        var danhSach = await _db.DanhMucs
            .Include(x => x.DanhMucCha)
            .OrderBy(x => x.ThuTu)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync();

        return View(danhSach);
    }

    // MỞ FORM THÊM
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapDanhMucCha();
        return View(new DanhMuc());
    }

    // LƯU DANH MỤC MỚI
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DanhMuc model)
    {
        model.TenDanhMuc = model.TenDanhMuc?.Trim() ?? "";

        // Tự sinh slug
        model.Slug = SlugHelper.Generate(model.TenDanhMuc);

        // Slug không nhập từ form nên phải bỏ lỗi validation cũ
        ModelState.Remove(nameof(DanhMuc.Slug));

        if (string.IsNullOrWhiteSpace(model.TenDanhMuc))
        {
            ModelState.AddModelError(
                nameof(DanhMuc.TenDanhMuc),
                "Vui lòng nhập tên danh mục.");
        }

        if (!ModelState.IsValid)
        {
            await NapDanhMucCha(model.DanhMucChaId);
            return View(model);
        }

        _db.DanhMucs.Add(model);
        await _db.SaveChangesAsync();

        TempData["Success"] =
            $"Đã thêm danh mục \"{model.TenDanhMuc}\" thành công.";

        return RedirectToAction(nameof(Index));
    }

    // MỞ FORM SỬA
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _db.DanhMucs.FindAsync(id);

        if (model == null)
            return NotFound();

        await NapDanhMucCha(model.DanhMucChaId, id);

        return View(model);
    }

    // LƯU DANH MỤC ĐÃ SỬA
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DanhMuc model)
    {
        if (id != model.Id)
            return NotFound();

        model.TenDanhMuc = model.TenDanhMuc?.Trim() ?? "";
        model.Slug = SlugHelper.Generate(model.TenDanhMuc);

        // Slug được sinh tự động
        ModelState.Remove(nameof(DanhMuc.Slug));

        if (model.DanhMucChaId == model.Id)
        {
            ModelState.AddModelError(
                nameof(DanhMuc.DanhMucChaId),
                "Danh mục không thể là cha của chính nó.");
        }

        if (string.IsNullOrWhiteSpace(model.TenDanhMuc))
        {
            ModelState.AddModelError(
                nameof(DanhMuc.TenDanhMuc),
                "Vui lòng nhập tên danh mục.");
        }

        if (!ModelState.IsValid)
        {
            await NapDanhMucCha(model.DanhMucChaId, id);
            return View(model);
        }

        var current = await _db.DanhMucs.FindAsync(id);

        if (current == null)
            return NotFound();

        current.TenDanhMuc = model.TenDanhMuc;
        current.Slug = model.Slug;
        current.DanhMucChaId = model.DanhMucChaId;
        current.ThuTu = model.ThuTu;

        await _db.SaveChangesAsync();

        TempData["Success"] =
            $"Đã cập nhật danh mục \"{model.TenDanhMuc}\".";

        return RedirectToAction(nameof(Index));
    }

    // DANH MỤC CHA
    private async Task NapDanhMucCha(
        int? selected = null,
        int? loaiId = null)
    {
        var query = _db.DanhMucs
            .AsNoTracking()
            .AsQueryable();

        if (loaiId.HasValue)
        {
            query = query.Where(x => x.Id != loaiId.Value);
        }

        var danhSach = await query
            .OrderBy(x => x.TenDanhMuc)
            .ToListAsync();

        ViewBag.DanhMucChaId = new SelectList(
            danhSach,
            "Id",
            "TenDanhMuc",
            selected);
    }
}