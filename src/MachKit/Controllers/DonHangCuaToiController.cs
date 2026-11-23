using MachKit.Models;
using MachKit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MachKit.Controllers;

[Authorize]
[Route("DonHangCuaToi")]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public sealed class DonHangCuaToiController : Controller
{
    private readonly DonHangQuanLyService _service;
    private readonly UserManager<ApplicationUser> _users;
    public DonHangCuaToiController(DonHangQuanLyService service, UserManager<ApplicationUser> users)
    {
        _service = service;
        _users = users;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? trangThai, int trang = 1,
        CancellationToken cancellationToken = default)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        return View(await _service.DanhSachCuaToiAsync(userId, trangThai, trang, cancellationToken));
    }

    [HttpGet("ChiTiet/{id:int}")]
    public async Task<IActionResult> ChiTiet(int id, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        if (id <= 0) return NotFound();
        var model = await _service.ChiTietCuaToiAsync(userId, id, cancellationToken);
        return model is null ? NotFound() : View(model);
    }
    // Không có POST hủy/sửa ở phía khách trong phạm vi Chặng 4D này.
}
