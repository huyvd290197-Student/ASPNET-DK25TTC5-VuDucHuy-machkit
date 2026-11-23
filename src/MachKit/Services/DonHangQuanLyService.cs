using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using MachKit.Data;
using MachKit.Helpers;
using MachKit.Models;
using MachKit.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MachKit.Services;

public sealed record CapNhatDonHangKetQua(bool ThanhCong, string ThongBao,
    bool ChuaRoKetQua = false, bool KhongTimThay = false);

public sealed class DonHangQuanLyService
{
    private readonly AppDbContext _db;
    private readonly ILogger<DonHangQuanLyService> _logger;

    public DonHangQuanLyService(AppDbContext db, ILogger<DonHangQuanLyService> logger)
    {
        _db = db;
        _logger = logger;
    }

    // Chỉ controller Admin có kiểm tra role được gọi các phương thức quản trị.
    public Task<DonHangDanhSachViewModel> DanhSachAdminAsync(
        string? tuKhoa, string? trangThai, int trang, CancellationToken ct = default)
        => DanhSachAsync(_db.DonHangs.AsNoTracking(), tuKhoa, trangThai, trang, ct);

    public Task<DonHangDanhSachViewModel> DanhSachCuaToiAsync(
        string userId, string? trangThai, int trang, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("Thiếu tài khoản đang đăng nhập.", nameof(userId));
        return DanhSachAsync(_db.DonHangs.AsNoTracking()
            .Where(x => x.NguoiDungId == userId), null, trangThai, trang, ct);
    }

    private static async Task<DonHangDanhSachViewModel> DanhSachAsync(
        IQueryable<DonHang> query, string? tuKhoa, string? trangThai,
        int trang, CancellationToken ct)
    {
        var key = tuKhoa?.Trim();
        if (key?.Length > 100) key = key[..100];
        var status = DonHang4DHelper.LaTrangThai(trangThai) ? trangThai : null;
        if (!string.IsNullOrWhiteSpace(key))
        {
            string ma = key.StartsWith("MK-", StringComparison.OrdinalIgnoreCase) ? key[3..] : key;
            int id = int.TryParse(ma, out var n) && n > 0 ? n : 0;
            query = query.Where(x => x.Id == id || x.TenNguoiNhan.Contains(key)
                || x.SoDienThoai.Contains(key));
        }
        if (status is not null) query = query.Where(x => x.TrangThai == status);
        int total = await query.CountAsync(ct);
        const int size = 10;
        int pages = Math.Max(1, (int)Math.Ceiling(total / (double)size));
        trang = Math.Clamp(trang, 1, pages);
        var rows = await query.OrderByDescending(x => x.NgayDatUtc).ThenByDescending(x => x.Id)
            .Skip((trang - 1) * size).Take(size)
            .Select(x => new DonHangTomTatViewModel
            {
                Id = x.Id, NgayDatUtc = x.NgayDatUtc, TenNguoiNhan = x.TenNguoiNhan,
                SoDienThoai = x.SoDienThoai, TrangThai = x.TrangThai,
                TongTien = x.TongTien, SoLoai = x.ChiTiets.Count
            }).ToListAsync(ct);
        return new()
        {
            TuKhoa = key, TrangThai = status, Trang = trang,
            TongSo = total, KichThuocTrang = size, Dons = rows
        };
    }

    public Task<DonHangChiTietViewModel?> ChiTietAdminAsync(int id, CancellationToken ct = default)
        => ChiTietAsync(_db.DonHangs.AsNoTracking().Where(x => x.Id == id), true, ct);

    public Task<DonHangChiTietViewModel?> ChiTietCuaToiAsync(
        string userId, int id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Task.FromResult<DonHangChiTietViewModel?>(null);
        // Kiểm tra quyền ở TRUY VẤN, không chỉ ẩn nút ở giao diện.
        return ChiTietAsync(_db.DonHangs.AsNoTracking()
            .Where(x => x.Id == id && x.NguoiDungId == userId), false, ct);
    }

    private async Task<DonHangChiTietViewModel?> ChiTietAsync(
        IQueryable<DonHang> query, bool admin, CancellationToken ct)
    {
        var don = await query.Include(x => x.ChiTiets).SingleOrDefaultAsync(ct);
        if (don is null) return null;
        var logs = _db.LichSuTrangThaiDonHangs.AsNoTracking()
            .Where(x => x.DonHangId == don.Id)
            .OrderBy(x => x.Id);
        List<NhatKyDonHangViewModel> history;
        if (admin)
        {
            history = await logs.Select(x => new NhatKyDonHangViewModel
            {
                ThoiDiemUtc = x.ThoiDiemUtc, TrangThaiCu = x.TrangThaiCu,
                TrangThaiMoi = x.TrangThaiMoi, DaHoanTon = x.DaHoanTon,
                GhiChu = x.GhiChu,
                NguoiThucHien = x.NguoiThucHien != null
                    ? (x.NguoiThucHien.UserName ?? x.NguoiThucHienId) : x.NguoiThucHienId
            }).ToListAsync(ct);
        }
        else
        {
            history = await logs.Select(x => new NhatKyDonHangViewModel
            {
                ThoiDiemUtc = x.ThoiDiemUtc, TrangThaiCu = x.TrangThaiCu,
                TrangThaiMoi = x.TrangThaiMoi, DaHoanTon = x.DaHoanTon
                // Cố ý không lấy GhiChu hoặc tài khoản nhân viên cho trang khách.
            }).ToListAsync(ct);
        }
        return new()
        {
            Don = don, LaQuanTri = admin, LichSu = history,
            CapNhat = new()
            {
                Id = don.Id, TrangThaiCu = don.TrangThai,
                TrangThaiMoi = DonHang4DHelper.BuocTiepTheo(don.TrangThai).FirstOrDefault() ?? "",
                MaThaoTac = Guid.NewGuid()
            }
        };
    }

    public async Task<CapNhatDonHangKetQua> CapNhatAsync(
        string actorId, CapNhatTrangThaiDonHangInput input, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(actorId)) return new(false, "Bạn cần đăng nhập lại.");
        // Sao chép + chuẩn hóa một lần, giữ cùng MaThaoTac qua mọi lần retry.
        var request = new CapNhatTrangThaiDonHangInput
        {
            Id = input.Id, MaThaoTac = input.MaThaoTac,
            TrangThaiCu = input.TrangThaiCu, TrangThaiMoi = input.TrangThaiMoi,
            GhiChu = string.IsNullOrWhiteSpace(input.GhiChu) ? null : input.GhiChu.Trim()
        };
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(request, new ValidationContext(request), errors, true))
            return new(false, string.Join(" ", errors.Select(x => x.ErrorMessage)));
        try
        {
            var strategy = _db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                ct.ThrowIfCancellationRequested();
                // Phương thức này chạy trong request riêng, không có thay đổi nghiệp vụ khác.
                _db.ChangeTracker.Clear();
                return await CapNhatMotLanAsync(actorId, request, ct);
            });
        }
        catch (DonHangKhongHopLeException ex)
        {
            _db.ChangeTracker.Clear();
            return new(false, ex.Message);
        }
        catch (Exception ex) when (ex is DbUpdateException or DbException or RetryLimitExceededException)
        {
            _db.ChangeTracker.Clear();
            _logger.LogError(ex, "Chưa rõ kết quả đổi trạng thái đơn {DonId}, thao tác {MaThaoTac}.",
                request.Id, request.MaThaoTac);
            // Không nói chắc đã rollback nếu mất kết nối ngay tại COMMIT.
            return new(false,
                "Chưa xác nhận được kết quả do SQL Server bận hoặc mất kết nối. " +
                "Hãy kiểm tra / gửi lại CÙNG thao tác. Nếu đã lưu, hệ thống không hoàn tồn lần nữa.",
                ChuaRoKetQua: true);
        }
    }

    private async Task<CapNhatDonHangKetQua> CapNhatMotLanAsync(
        string actorId, CapNhatTrangThaiDonHangInput request, CancellationToken ct)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        int id = request.Id;
        // SQL Server: khóa đơn đến hết transaction để hai nhân viên không cùng xử lý trạng thái cũ.
        // Dữ liệu được tham số hóa, không nối chuỗi SQL từ form.
        var don = await _db.DonHangs.FromSqlInterpolated($@"
            SELECT * FROM [dbo].[DonHang] WITH (UPDLOCK, HOLDLOCK) WHERE [Id] = {id}")
            .AsNoTracking().SingleOrDefaultAsync(ct);
        if (don is null) return new(false, "Không tìm thấy đơn hàng.", KhongTimThay: true);

        var previous = await _db.LichSuTrangThaiDonHangs.AsNoTracking()
            .SingleOrDefaultAsync(x => x.MaThaoTac == request.MaThaoTac, ct);
        if (previous is not null)
        {
            if (previous.DonHangId != id || previous.NguoiThucHienId != actorId
                || previous.TrangThaiCu != request.TrangThaiCu
                || previous.TrangThaiMoi != request.TrangThaiMoi || previous.GhiChu != request.GhiChu)
                throw new DonHangKhongHopLeException("Mã thao tác đã được sử dụng cho nội dung khác. Hãy tải lại chi tiết đơn.");
            await tx.CommitAsync(ct);
            return new(true, "Thao tác này đã được lưu trước đó. Không ghi thêm nhật ký hoặc thay đổi tồn lần nữa.");
        }

        // Chỉ 4C đảm bảo trừ tồn lúc tạo đơn. Không suy đoán tồn của đơn nhập tay/đơn 4A cũ.
        if (!don.MaYeuCau.HasValue || don.MaYeuCau.Value == Guid.Empty)
            throw new DonHangKhongHopLeException(
                "Đơn cũ thiếu MaYeuCau của 4C: chỉ cho xem. Cần đối chiếu dữ liệu kho, không tự thêm mã để mở khóa.");
        if (don.TrangThai != request.TrangThaiCu)
            throw new DonHangKhongHopLeException(
                "Trạng thái vừa thay đổi ở cửa sổ khác. Trang đã được tải lại; hãy kiểm tra trước khi thao tác tiếp.");
        if (!DonHang4DHelper.DuocChuyen(don.TrangThai, request.TrangThaiMoi))
            throw new DonHangKhongHopLeException("Không được nhảy bước, quay lùi hoặc mở lại đơn đã kết thúc.");

        bool hoanTon = request.TrangThaiMoi == TrangThaiDonHang.DaHuy;
        if (hoanTon)
        {
            if (await _db.LichSuTrangThaiDonHangs.AnyAsync(
                x => x.DonHangId == id && x.TrangThaiMoi == TrangThaiDonHang.DaHuy, ct))
                throw new DonHangKhongHopLeException("Đơn đã có nhật ký hủy. Dừng để đối chiếu dữ liệu; không hoàn tồn thêm.");

            var details = await _db.ChiTietDonHangs.FromSqlInterpolated($@"
                SELECT * FROM [dbo].[ChiTietDonHang] WITH (HOLDLOCK) WHERE [DonHangId] = {id}")
                .AsNoTracking().ToListAsync(ct);
            if (details.Count == 0 || details.Any(x => x.LinhKienId <= 0 || x.SoLuong <= 0))
                throw new DonHangKhongHopLeException("Chi tiết đơn không hợp lệ. Không hủy hoặc hoàn tồn tự động.");

            // Khóa kho theo LinhKienId tăng dần, cùng thứ tự với checkout 4C.
            foreach (var group in details.GroupBy(x => x.LinhKienId).OrderBy(x => x.Key))
            {
                long sum = group.Sum(x => (long)x.SoLuong);
                if (sum > int.MaxValue)
                    throw new DonHangKhongHopLeException("Số lượng hoàn tồn vượt giới hạn lưu trữ.");
                int qty = (int)sum;
                int productId = group.Key;
                var product = await _db.LinhKiens.FromSqlInterpolated($@"
                    SELECT * FROM [dbo].[LinhKien] WITH (UPDLOCK, HOLDLOCK) WHERE [Id] = {productId}")
                    .AsNoTracking().SingleOrDefaultAsync(ct);
                if (product is null || product.SoLuongTon < 0 || product.SoLuongTon > int.MaxValue - qty)
                    throw new DonHangKhongHopLeException("Không thể hoàn tồn: linh kiện thiếu hoặc tồn không hợp lệ/vượt giới hạn.");

                // Vẫn hoàn cho sản phẩm ngừng bán; không tự mở bán lại.
                int changed = await _db.LinhKiens
                    .Where(x => x.Id == productId && x.SoLuongTon >= 0 && x.SoLuongTon <= int.MaxValue - qty)
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.SoLuongTon, x => x.SoLuongTon + qty), ct);
                if (changed != 1)
                    throw new DonHangKhongHopLeException("Kho vừa thay đổi, hủy đơn chưa hoàn tất. Hãy kiểm tra lại.");
            }
        }

        int updated = await _db.DonHangs
            .Where(x => x.Id == id && x.TrangThai == request.TrangThaiCu)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.TrangThai, request.TrangThaiMoi), ct);
        if (updated != 1)
            throw new DonHangKhongHopLeException("Không cập nhật được trạng thái. Giao dịch này bị hủy.");

        _db.LichSuTrangThaiDonHangs.Add(new LichSuTrangThaiDonHang
        {
            DonHangId = id, MaThaoTac = request.MaThaoTac, NguoiThucHienId = actorId,
            TrangThaiCu = request.TrangThaiCu, TrangThaiMoi = request.TrangThaiMoi,
            GhiChu = request.GhiChu, ThoiDiemUtc = DateTime.UtcNow, DaHoanTon = hoanTon
        });
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return new(true, hoanTon
            ? "Đã hủy đơn và hoàn tồn một lần. Không phục hồi giỏ hoặc thay đổi giá trị đơn đã lưu."
            : $"Đã chuyển sang {DonHang4DHelper.TenTrangThai(request.TrangThaiMoi)}. Không trừ tồn thêm.");
        // Dispose transaction khi có lỗi trước commit sẽ rollback cả tồn + trạng thái + nhật ký.
    }

    private sealed class DonHangKhongHopLeException : Exception
    {
        public DonHangKhongHopLeException(string message) : base(message) { }
    }
}
