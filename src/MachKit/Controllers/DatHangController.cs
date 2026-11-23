using MachKit.Models;
using MachKit.Services;
using MachKit.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MachKit.Controllers;

[Authorize]
[Route("DatHang")]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class DatHangController : Controller
{
    private readonly GioHangService _gioHang;
    private readonly UserManager<ApplicationUser> _users;

    public DatHangController(GioHangService gioHang, UserManager<ApplicationUser> users)
    {
        _gioHang = gioHang;
        _users = users;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();

        var model = await NapTrangAsync(userId, new ThongTinNhanHangInput(), cancellationToken);
        if (model.GioHang.Dongs.Count == 0)
        {
            TempData["GioHangLoi"] = "Giỏ hàng đang trống. Hãy chọn linh kiện trước khi đặt hàng.";
            return RedirectToAction("Index", "GioHang", new { area = "" });
        }

        KiemTraGio(model.GioHang);
        return View(model);
    }

    [HttpPost("KiemTra")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KiemTra(
        [FromForm] ThongTinNhanHangInput input, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();

        // LUON doc lai gio, gia, ton va NgungBan tu SQL Server, ke ca khi form sai.
        // Khong doc bat ky userId/gia/so luong nao do trinh duyet gui len.
        var model = await NapTrangAsync(userId, input, cancellationToken);
        KiemTraGio(model.GioHang);

        if (!ModelState.IsValid) return View("Index", model);

        // 4B chi xem lai. KHONG SaveChanges, tru ton, xoa gio hay tao DonHang.
        return View("XemLai", model);
    }

    [HttpPost("SuaThongTin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SuaThongTin(
        [FromForm] ThongTinNhanHangInput input, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();

        var model = await NapTrangAsync(userId, input, cancellationToken);
        KiemTraGio(model.GioHang);
        return View("Index", model);
    }

    // Khi mo nham URL POST hoac quay lai sau dang nhap, hien lai form GET.
    [HttpGet("KiemTra")]
    [HttpGet("SuaThongTin")]
    public IActionResult QuayLaiForm() => RedirectToAction(nameof(Index));

    private async Task<DatHangViewModel> NapTrangAsync(
        string userId, ThongTinNhanHangInput input, CancellationToken cancellationToken)
    {
        return new DatHangViewModel
        {
            TenNguoiNhan = input.TenNguoiNhan?.Trim() ?? "",
            SoDienThoai = input.SoDienThoai?.Trim() ?? "",
            DiaChiGiaoHang = input.DiaChiGiaoHang?.Trim() ?? "",
            GhiChu = string.IsNullOrWhiteSpace(input.GhiChu) ? null : input.GhiChu.Trim(),
            GioHang = await _gioHang.LayAsync(userId, cancellationToken)
        };
    }

    private void KiemTraGio(GioHangViewModel gio)
    {
        if (gio.Dongs.Count == 0)
        {
            ModelState.AddModelError(string.Empty,
                "Giỏ hàng đang trống hoặc vừa bị xóa ở cửa sổ khác. Hãy chọn lại linh kiện.");
            return;
        }

        foreach (var dong in gio.Dongs)
        {
            if (dong.CanhBao is string canhBao)
                ModelState.AddModelError(string.Empty, $"{dong.TenLinhKien}: {canhBao}");
        }
    }
}
