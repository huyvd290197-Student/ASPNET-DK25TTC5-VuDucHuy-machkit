using MachKit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Data;

public static class SeedData
{
    public static async Task InitAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var config = services.GetRequiredService<IConfiguration>();

        foreach (string role in new[] { "Admin", "NhanVien", "KhachHang" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        string? adminEmail = config["AdminSeed:Email"];
        string? adminPassword = config["AdminSeed:Password"];

        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    HoTen = "Quản trị viên MạchKit",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
            else if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        if (await db.LinhKiens.AnyAsync()) return;

        var dmLed = new DanhMuc { TenDanhMuc = "LED", Slug = "led", ThuTu = 1 };
        var dmDienTro = new DanhMuc { TenDanhMuc = "Điện trở", Slug = "dien-tro", ThuTu = 2 };
        var dmBreadboard = new DanhMuc { TenDanhMuc = "Breadboard", Slug = "breadboard", ThuTu = 3 };
        db.DanhMucs.AddRange(dmLed, dmDienTro, dmBreadboard);

        var oem = new ThuongHieu { TenThuongHieu = "OEM", QuocGia = "Trung Quốc" };
        db.ThuongHieus.Add(oem);
        await db.SaveChangesAsync();

        var led = new LinhKien
        {
            MaLinhKien = "LK0001", TenLinhKien = "LED đỏ 5mm", Slug = "led-do-5mm",
            MoTa = "LED đỏ 5mm dùng cho các mạch thực hành cơ bản.", Gia = 2000,
            SoLuongTon = 10, MucTonToiThieu = 5, DanhMucId = dmLed.Id,
            ThuongHieuId = oem.Id, HinhAnh = "/images/led-do.jpg"
        };
        var r330 = new LinhKien
        {
            MaLinhKien = "LK0002", TenLinhKien = "Điện trở 330 Ω", Slug = "dien-tro-330-ohm",
            MoTa = "Điện trở 330 Ω dùng hạn dòng cho LED.", Gia = 1000,
            SoLuongTon = 7, MucTonToiThieu = 5, DanhMucId = dmDienTro.Id,
            ThuongHieuId = oem.Id, HinhAnh = "/images/dien-tro.jpg"
        };
        var bb = new LinhKien
        {
            MaLinhKien = "LK0003", TenLinhKien = "Breadboard mini", Slug = "breadboard-mini",
            MoTa = "Breadboard mini dùng lắp mạch thử nghiệm không cần hàn.", Gia = 15000,
            SoLuongTon = 5, MucTonToiThieu = 2, DanhMucId = dmBreadboard.Id,
            ThuongHieuId = oem.Id, HinhAnh = "/images/breadboard.jpg"
        };
        db.LinhKiens.AddRange(led, r330, bb);
        await db.SaveChangesAsync();

        db.ThongSoKyThuats.AddRange(
            new ThongSoKyThuat { LinhKienId = led.Id, TenThongSo = "Đường kính", GiaTri = "5", DonVi = "mm" },
            new ThongSoKyThuat { LinhKienId = led.Id, TenThongSo = "Màu ánh sáng", GiaTri = "Đỏ" },
            new ThongSoKyThuat { LinhKienId = r330.Id, TenThongSo = "Điện trở", GiaTri = "330", DonVi = "Ω" },
            new ThongSoKyThuat { LinhKienId = r330.Id, TenThongSo = "Công suất", GiaTri = "0.25", DonVi = "W" },
            new ThongSoKyThuat { LinhKienId = bb.Id, TenThongSo = "Loại", GiaTri = "Mini" }
        );

        var bo = new BoThucHanh
        {
            MaBo = "MK-01", TenBo = "Đèn LED đôi trên breadboard",
            MoTa = "Bộ thực hành mạch hai đèn LED trên breadboard.",
            HinhAnh = "/images/hero-machkit.jpg", KichHoat = true
        };
        db.BoThucHanhs.Add(bo);
        await db.SaveChangesAsync();

        db.ThanhPhanBos.AddRange(
            new ThanhPhanBo { BoThucHanhId = bo.Id, LinhKienId = led.Id, SoLuong = 2 },
            new ThanhPhanBo { BoThucHanhId = bo.Id, LinhKienId = r330.Id, SoLuong = 2 },
            new ThanhPhanBo { BoThucHanhId = bo.Id, LinhKienId = bb.Id, SoLuong = 1 }
        );
        await db.SaveChangesAsync();
    }
}
