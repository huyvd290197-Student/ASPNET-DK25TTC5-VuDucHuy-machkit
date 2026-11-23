using MachKit.Models;
using MachKit.Services;
using MachKit.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MachKit.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,NhanVien")]
[Route("Admin/DonHang")]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public sealed class DonHangController : Controller
{
    private readonly DonHangQuanLyService _service;
    private readonly UserManager<ApplicationUser> _users;
    public DonHangController(DonHangQuanLyService service, UserManager<ApplicationUser> users)
    {
        _service = service;
        _users = users;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? tuKhoa, string? trangThai,
        int trang = 1, CancellationToken cancellationToken = default)
        => View(await _service.DanhSachAdminAsync(tuKhoa, trangThai, trang, cancellationToken));

    [HttpGet("ChiTiet/{id:int}")]
    public async Task<IActionResult> ChiTiet(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return NotFound();
        var model = await _service.ChiTietAdminAsync(id, cancellationToken);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost("CapNhatTrangThai")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CapNhatTrangThai(
        [FromForm] CapNhatTrangThaiDonHangInput input, CancellationToken cancellationToken)
    {
        string? actorId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(actorId)) return Challenge();
        if (input.Id <= 0) return BadRequest();
        if (!ModelState.IsValid)
        {
            var model = await _service.ChiTietAdminAsync(input.Id, cancellationToken);
            if (model is null) return NotFound();
            // Giữ lý do hợp lệ; dùng lại trạng thái hiện tại + mã mới sau lỗi dữ liệu nhập.
            model.CapNhat.GhiChu = input.GhiChu;
            model.CapNhat.TrangThaiMoi = input.TrangThaiMoi;
            return View("ChiTiet", model);
        }
        var result = await _service.CapNhatAsync(actorId, input, cancellationToken);
        if (result.KhongTimThay) return NotFound();
        if (result.ChuaRoKetQua)
        {
            return View("ThuLai", new CapNhatDonHangThuLaiViewModel
            {
                YeuCau = input, ThongBao = result.ThongBao
            });
        }
        TempData[result.ThanhCong ? "DonHang4DThanhCong" : "DonHang4DLoi"] = result.ThongBao;
        // PRG: F5 trang chi tiết không gửi lại POST.
        return RedirectToAction(nameof(ChiTiet), new { area = "Admin", id = input.Id });
    }
}
