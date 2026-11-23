using MachKit.Models;
using MachKit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MachKit.Controllers;

[Authorize]
[Route("GioHang")]
public class GioHangController : Controller
{
    private readonly GioHangService _gioHang;
    private readonly UserManager<ApplicationUser> _users;

    public GioHangController(GioHangService gioHang, UserManager<ApplicationUser> users)
    {
        _gioHang = gioHang;
        _users = users;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        return View(await _gioHang.LayAsync(userId, cancellationToken));
    }

    [HttpPost("Them")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Them(int linhKienId, int soLuong = 1,
        CancellationToken cancellationToken = default)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        if (!ModelState.IsValid)
            return ThongBao(new(false, "Số lượng phải là số nguyên hợp lệ."));
        return ThongBao(await _gioHang.ThemAsync(userId, linhKienId, soLuong, cancellationToken));
    }

    [HttpPost("CapNhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CapNhat(int id, int soLuong, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        if (!ModelState.IsValid)
            return ThongBao(new(false, "Số lượng phải là số nguyên hợp lệ."));
        return ThongBao(await _gioHang.CapNhatAsync(userId, id, soLuong, cancellationToken));
    }

    [HttpPost("Xoa")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(int id, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        if (!ModelState.IsValid)
            return ThongBao(new(false, "Thông tin dòng giỏ hàng không hợp lệ."));
        return ThongBao(await _gioHang.XoaAsync(userId, id, cancellationToken));
    }

    private IActionResult ThongBao(GioHangKetQua ketQua)
    {
        TempData[ketQua.ThanhCong ? "GioHangThanhCong" : "GioHangLoi"] = ketQua.ThongBao;
        // POST -> redirect -> GET: tải lại trang không tự gửi thêm sản phẩm lần nữa.
        return RedirectToAction(nameof(Index));
    }
}
