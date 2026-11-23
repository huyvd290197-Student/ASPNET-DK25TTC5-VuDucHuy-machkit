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
    private readonly DatHangService _datHang;
    private readonly UserManager<ApplicationUser> _users;

    public DatHangController(GioHangService gioHang, DatHangService datHang,
        UserManager<ApplicationUser> users)
    {
        _gioHang = gioHang;
        _datHang = datHang;
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
        // Giữ hành vi 4B: dữ liệu nhập sai vẫn hiện lại giỏ thật.
        var model = await NapTrangAsync(userId, input, cancellationToken);
        KiemTraGio(model.GioHang);
        if (!ModelState.IsValid) return View("Index", model);

        // Phát phiếu từ đúng lần đọc dùng cho màn hình xem lại, chưa giữ hàng.
        var ketQua = await _datHang.ChuanBiAsync(userId, input, cancellationToken);
        if (ketQua.Loi is not null)
        {
            ModelState.AddModelError(string.Empty, ketQua.Loi);
            return View("Index", ketQua.Trang);
        }
        return View("XemLai", ketQua.Trang);
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

    [HttpPost("XacNhan")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> XacNhan(
        [FromForm] XacNhanDatHangInput input, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        if (!ModelState.IsValid)
        {
            var invalidModel = await NapTrangAsync(userId, new ThongTinNhanHangInput(), cancellationToken);
            return View("Index", invalidModel);
        }

        var result = await _datHang.XacNhanAsync(userId, input.PhieuXacNhan, cancellationToken);
        if (result.DonHangId is int id)
            // POST -> Redirect -> GET: F5 trang kết quả không gửi thêm đơn.
            return RedirectToAction(nameof(ThanhCong), new { area = "", id });

        if (result.ThuLaiCungYeuCau)
        {
            // Giữ nguyên phiếu khi kết quả COMMIT có thể chưa xác định.
            // Không sinh mã mới và không khẳng định giỏ/kho đã rollback.
            return View("ThuLai", new DatHangThuLaiViewModel
            {
                PhieuXacNhan = input.PhieuXacNhan,
                ThongBao = result.Loi ?? "Chưa xác nhận được kết quả. Vui lòng thử lại cùng yêu cầu."
            });
        }

        ModelState.Clear();
        ModelState.AddModelError(string.Empty, result.Loi ?? "Không thể xác nhận đơn. Hãy kiểm tra lại giỏ.");
        var model = await NapTrangAsync(userId,
            result.NguoiNhan ?? new ThongTinNhanHangInput(), cancellationToken);
        KiemTraGio(model.GioHang);
        return View("Index", model);
    }

    [HttpGet("ThanhCong/{id:int}")]
    public async Task<IActionResult> ThanhCong(int id, CancellationToken cancellationToken)
    {
        string? userId = _users.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId)) return Challenge();
        if (id <= 0) return NotFound();
        // Không chỉ tìm theo Id; khách B không xem được địa chỉ/đơn của khách A.
        var don = await _datHang.LayDonCuaToiAsync(userId, id, cancellationToken);
        if (don is null) return NotFound();
        return View(don);
    }

    [HttpGet("KiemTra")]
    [HttpGet("SuaThongTin")]
    [HttpGet("XacNhan")]
    public IActionResult QuayLaiForm() => RedirectToAction(nameof(Index));

    private async Task<DatHangViewModel> NapTrangAsync(
        string userId, ThongTinNhanHangInput input, CancellationToken cancellationToken)
        => new()
        {
            TenNguoiNhan = input.TenNguoiNhan?.Trim() ?? "",
            SoDienThoai = input.SoDienThoai?.Trim() ?? "",
            DiaChiGiaoHang = input.DiaChiGiaoHang?.Trim() ?? "",
            GhiChu = string.IsNullOrWhiteSpace(input.GhiChu) ? null : input.GhiChu.Trim(),
            GioHang = await _gioHang.LayAsync(userId, cancellationToken)
        };

    private void KiemTraGio(GioHangViewModel gio)
    {
        if (gio.Dongs.Count == 0)
        {
            ModelState.AddModelError(string.Empty,
                "Giỏ đang trống hoặc đã được xử lý ở cửa sổ khác. Hãy kiểm tra lại giỏ.");
            return;
        }
        foreach (var dong in gio.Dongs)
            if (dong.CanhBao is string canhBao)
                ModelState.AddModelError(string.Empty, $"{dong.TenLinhKien}: {canhBao}");
    }
}
