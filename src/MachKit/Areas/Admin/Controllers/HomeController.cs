using MachKit.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.SoLinhKien = await _db.LinhKiens.CountAsync();
        ViewBag.SoDanhMuc = await _db.DanhMucs.CountAsync();
        ViewBag.SoThuongHieu = await _db.ThuongHieus.CountAsync();
        ViewBag.SoBo = await _db.BoThucHanhs.CountAsync();
        return View();
    }
}
