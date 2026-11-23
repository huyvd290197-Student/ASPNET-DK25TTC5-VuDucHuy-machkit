using MachKit.Data;
using MachKit.Models;
using MachKit.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Services;

public sealed record GioHangKetQua(bool ThanhCong, string ThongBao);

public class GioHangService
{
    private readonly AppDbContext _db;
    public GioHangService(AppDbContext db) => _db = db;

    public async Task<GioHangViewModel> LayAsync(
        string nguoiDungId, CancellationToken cancellationToken = default)
    {
        var dongs = await _db.GioHangItems.AsNoTracking()
            .Where(x => x.NguoiDungId == nguoiDungId)
            .OrderBy(x => x.Id)
            .Select(x => new GioHangDongViewModel
            {
                Id = x.Id,
                LinhKienId = x.LinhKienId,
                MaLinhKien = x.LinhKien!.MaLinhKien,
                TenLinhKien = x.LinhKien!.TenLinhKien,
                Slug = x.LinhKien!.Slug,
                HinhAnh = x.LinhKien!.HinhAnh,
                SoLuong = x.SoLuong,
                TonKho = x.LinhKien!.SoLuongTon,
                DonGia = x.LinhKien!.Gia,
                NgungBan = x.LinhKien!.NgungBan
            }).ToListAsync(cancellationToken);

        return new GioHangViewModel { Dongs = dongs };
    }

    public async Task<GioHangKetQua> ThemAsync(
        string nguoiDungId, int linhKienId, int soLuong,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nguoiDungId)) return new(false, "Bạn cần đăng nhập.");
        if (linhKienId <= 0 || soLuong <= 0)
            return new(false, "Linh kiện và số lượng phải hợp lệ; số lượng ít nhất là 1.");

        var sp = await _db.LinhKiens.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == linhKienId, cancellationToken);
        if (sp is null || sp.NgungBan) return new(false, "Linh kiện không tồn tại hoặc đã ngừng bán.");
        if (sp.Gia < 0) return new(false, "Giá linh kiện chưa hợp lệ. Vui lòng liên hệ quản trị.");
        if (sp.SoLuongTon <= 0) return new(false, "Linh kiện đã hết hàng.");

        var dong = await _db.GioHangItems.SingleOrDefaultAsync(
            x => x.NguoiDungId == nguoiDungId && x.LinhKienId == linhKienId,
            cancellationToken);

        // Dùng long để phép cộng không tràn số nguyên int.
        long tongSoLuong = (long)(dong?.SoLuong ?? 0) + soLuong;
        if (tongSoLuong > sp.SoLuongTon || tongSoLuong > int.MaxValue)
            return new(false, $"Kho còn {sp.SoLuongTon}; giỏ đang có {dong?.SoLuong ?? 0}. Không thể thêm {soLuong}.");

        if (dong is null)
        {
            _db.GioHangItems.Add(new GioHangItem
            {
                NguoiDungId = nguoiDungId,
                LinhKienId = linhKienId,
                SoLuong = (int)tongSoLuong,
                CapNhatUtc = DateTime.UtcNow
            });
        }
        else
        {
            dong.SoLuong = (int)tongSoLuong;
            dong.CapNhatUtc = DateTime.UtcNow;
        }

        return await LuuAsync("Đã thêm linh kiện vào giỏ hàng.", cancellationToken);
    }

    public async Task<GioHangKetQua> CapNhatAsync(
        string nguoiDungId, int id, int soLuong,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nguoiDungId)) return new(false, "Bạn cần đăng nhập.");
        if (id <= 0 || soLuong <= 0)
            return new(false, "Số lượng phải là số nguyên ít nhất bằng 1. Dùng nút Xóa để bỏ sản phẩm.");

        // Luôn kiểm tra chủ sở hữu, không tìm chỉ bằng Id từ trình duyệt.
        var dong = await _db.GioHangItems.Include(x => x.LinhKien)
            .SingleOrDefaultAsync(x => x.Id == id && x.NguoiDungId == nguoiDungId,
                cancellationToken);
        if (dong is null) return new(false, "Không tìm thấy dòng hàng trong giỏ của bạn.");
        var sp = dong.LinhKien;
        if (sp is null || sp.NgungBan) return new(false, "Linh kiện đã ngừng bán. Hãy xóa khỏi giỏ.");
        if (sp.Gia < 0) return new(false, "Giá linh kiện chưa hợp lệ. Vui lòng liên hệ quản trị.");
        if (soLuong > sp.SoLuongTon)
            return new(false, $"Kho hiện chỉ còn {Math.Max(0, sp.SoLuongTon)} linh kiện. Số lượng chưa được thay đổi.");

        dong.SoLuong = soLuong;
        dong.CapNhatUtc = DateTime.UtcNow;
        return await LuuAsync("Đã cập nhật số lượng.", cancellationToken);
    }

    public async Task<GioHangKetQua> XoaAsync(
        string nguoiDungId, int id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nguoiDungId)) return new(false, "Bạn cần đăng nhập.");
        var dong = await _db.GioHangItems.SingleOrDefaultAsync(
            x => x.Id == id && x.NguoiDungId == nguoiDungId, cancellationToken);
        if (dong is null) return new(false, "Không tìm thấy dòng hàng trong giỏ của bạn.");
        _db.GioHangItems.Remove(dong);
        return await LuuAsync("Đã xóa linh kiện khỏi giỏ hàng.", cancellationToken);
    }

    private async Task<GioHangKetQua> LuuAsync(string thanhCong, CancellationToken cancellationToken)
    {
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            return new(true, thanhCong);
        }
        catch (DbUpdateConcurrencyException)
        {
            _db.ChangeTracker.Clear();
            return new(false, "Giỏ vừa được thay đổi ở yêu cầu khác. Hãy xem lại số lượng và thử lại.");
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627))
        {
            // Hai yêu cầu đồng thời tạo cùng sản phẩm: unique index chặn dòng trùng.
            _db.ChangeTracker.Clear();
            return new(false, "Linh kiện vừa được thêm ở yêu cầu khác. Hãy xem lại giỏ trước khi thêm tiếp.");
        }
    }
}
