using MachKit.Models;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Data;

public static class SeedData
{
    public static async Task InitAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AppDbContext>();

        // Nếu đã có dữ liệu thì không tạo lại.
        if (await db.LinhKiens.AnyAsync())
            return;

        // 1. Danh mục
        var dmLed = new DanhMuc
        {
            TenDanhMuc = "LED",
            Slug = "led",
            ThuTu = 1
        };

        var dmDienTro = new DanhMuc
        {
            TenDanhMuc = "Điện trở",
            Slug = "dien-tro",
            ThuTu = 2
        };

        var dmBreadboard = new DanhMuc
        {
            TenDanhMuc = "Breadboard",
            Slug = "breadboard",
            ThuTu = 3
        };

        db.DanhMucs.AddRange(
            dmLed,
            dmDienTro,
            dmBreadboard
        );

        // 2. Thương hiệu
        var oem = new ThuongHieu
        {
            TenThuongHieu = "OEM",
            QuocGia = "Trung Quốc"
        };

        db.ThuongHieus.Add(oem);

        await db.SaveChangesAsync();

        // 3. Linh kiện
        var ledDo = new LinhKien
        {
            MaLinhKien = "LK0001",
            TenLinhKien = "LED đỏ 5mm",
            Slug = "led-do-5mm",
            MoTa = "LED đỏ 5mm dùng cho các mạch thực hành cơ bản.",
            Gia = 2000,
            SoLuongTon = 10,
            MucTonToiThieu = 5,
            DanhMucId = dmLed.Id,
            ThuongHieuId = oem.Id,
            NgungBan = false
        };

        var dienTro = new LinhKien
        {
            MaLinhKien = "LK0002",
            TenLinhKien = "Điện trở 330 Ohm",
            Slug = "dien-tro-330-ohm",
            MoTa = "Điện trở 330 Ohm dùng hạn dòng cho LED.",
            Gia = 1000,
            SoLuongTon = 7,
            MucTonToiThieu = 5,
            DanhMucId = dmDienTro.Id,
            ThuongHieuId = oem.Id,
            NgungBan = false
        };

        var breadboard = new LinhKien
        {
            MaLinhKien = "LK0003",
            TenLinhKien = "Breadboard mini",
            Slug = "breadboard-mini",
            MoTa = "Breadboard mini dùng lắp mạch thử nghiệm.",
            Gia = 15000,
            SoLuongTon = 5,
            MucTonToiThieu = 2,
            DanhMucId = dmBreadboard.Id,
            ThuongHieuId = oem.Id,
            NgungBan = false
        };

        db.LinhKiens.AddRange(
            ledDo,
            dienTro,
            breadboard
        );

        await db.SaveChangesAsync();

        // 4. Bộ thực hành
        var boLed = new BoThucHanh
        {
            MaBo = "BO001",
            TenBo = "Đèn LED đôi trên breadboard",
            MoTa = "Bộ linh kiện thực hành mạch hai đèn LED.",
            KichHoat = true
        };

        db.BoThucHanhs.Add(boLed);

        await db.SaveChangesAsync();

        // 5. Thành phần của một bộ
        db.ThanhPhanBos.AddRange(
            new ThanhPhanBo
            {
                BoThucHanhId = boLed.Id,
                LinhKienId = ledDo.Id,
                SoLuong = 2
            },
            new ThanhPhanBo
            {
                BoThucHanhId = boLed.Id,
                LinhKienId = dienTro.Id,
                SoLuong = 2
            },
            new ThanhPhanBo
            {
                BoThucHanhId = boLed.Id,
                LinhKienId = breadboard.Id,
                SoLuong = 1
            }
        );

        await db.SaveChangesAsync();
    }
}