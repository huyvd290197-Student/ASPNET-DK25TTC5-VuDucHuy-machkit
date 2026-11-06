using System.Globalization;
using MachKit.Models;
using Microsoft.AspNetCore.Mvc;

namespace MachKit.Controllers;

public sealed class HomeController : Controller
{
    // Dữ liệu giả lập cho buổi đầu; chưa kết nối SQL Server.
    private static readonly LinhKienMau[] CauThanh =
    {
        new("LED-DO", "LED đỏ", 2, 10, 2000m),
        new("R-330", "Điện trở 330 Ω", 2, 7, 1000m),
        new("BB-MINI", "Breadboard mini", 1, 5, 15000m)
    };

    [HttpGet]
    public IActionResult Index(string? soBo)
    {
        // Lần đầu mở trang: 1 bộ. Gửi ô trống: báo lỗi thay vì tự tính 1 bộ.
        string nhap = Request.Query.ContainsKey("soBo") ? (soBo?.Trim() ?? "") : "1";
        bool hopLe = int.TryParse(nhap, NumberStyles.None, CultureInfo.InvariantCulture, out int n)
                     && n >= 1 && n <= 99;
        return View(new DuToanBo
        {
            GiaTriNhap = nhap,
            Loi = hopLe ? null : "Hãy nhập số bộ là số nguyên từ 1 đến 99.",
            Dong = hopLe ? CauThanh.Select(x => new DongDuToan(x, n)).ToArray() : Array.Empty<DongDuToan>(),
            // Phép chia nguyên: 7 điện trở / 2 chiếc mỗi bộ chỉ ghép được 3 bộ.
            SoBoToiDa = CauThanh.Min(x => x.Ton / x.DinhMuc)
        });
    }
}
