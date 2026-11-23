using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text.Json;
using MachKit.Data;
using MachKit.Models;
using MachKit.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MachKit.Services;

public sealed record DatHangChuanBiKetQua(DatHangViewModel Trang, string? Loi);

public sealed record DatHangKetQua(
    int? DonHangId,
    string? Loi = null,
    bool ThuLaiCungYeuCau = false,
    ThongTinNhanHangInput? NguoiNhan = null);

public sealed class DatHangService
{
    // Khớp precision(18,0) của DonHang và ChiTietDonHang trong Chặng 4A.
    private const decimal TienToiDa = 999999999999999999m;
    private const int ToiDaSoLoai = 200;
    private readonly AppDbContext _db;
    private readonly IDataProtector _protector;
    private readonly ILogger<DatHangService> _logger;

    public DatHangService(AppDbContext db, IDataProtectionProvider protection,
        ILogger<DatHangService> logger)
    {
        _db = db;
        _protector = protection.CreateProtector("MachKit.DatHang.PhieuXacNhan.v1");
        _logger = logger;
    }

    // Nạp lại dữ liệu một lần để ảnh chụp được bảo vệ KHỚP với màn hình xem lại.
    // Không ghi database hoặc giữ tồn kho ở bước này.
    public async Task<DatHangChuanBiKetQua> ChuanBiAsync(
        string userId, ThongTinNhanHangInput input, CancellationToken ct = default)
    {
        var nguoiNhan = ChuanHoa(input);
        var model = new DatHangViewModel
        {
            TenNguoiNhan = nguoiNhan.TenNguoiNhan,
            SoDienThoai = nguoiNhan.SoDienThoai,
            DiaChiGiaoHang = nguoiNhan.DiaChiGiaoHang,
            GhiChu = nguoiNhan.GhiChu
        };
        if (string.IsNullOrWhiteSpace(userId))
            return new(model, "Bạn cần đăng nhập.");

        var rows = await _db.GioHangItems.AsNoTracking()
            .Where(x => x.NguoiDungId == userId)
            .OrderBy(x => x.LinhKienId)
            .Select(x => new DongXemLai
            {
                Id = x.Id,
                LinhKienId = x.LinhKienId,
                SoLuong = x.SoLuong,
                RowVersion = x.RowVersion,
                Ma = x.LinhKien!.MaLinhKien,
                Ten = x.LinhKien!.TenLinhKien,
                Slug = x.LinhKien!.Slug,
                Anh = x.LinhKien!.HinhAnh,
                Gia = x.LinhKien!.Gia,
                Ton = x.LinhKien!.SoLuongTon,
                NgungBan = x.LinhKien!.NgungBan
            }).Take(ToiDaSoLoai + 1).ToListAsync(ct);

        model.GioHang.Dongs = rows.Select(x => new GioHangDongViewModel
        {
            Id = x.Id, LinhKienId = x.LinhKienId, MaLinhKien = x.Ma,
            TenLinhKien = x.Ten, Slug = x.Slug, HinhAnh = x.Anh,
            SoLuong = x.SoLuong, TonKho = x.Ton, DonGia = x.Gia, NgungBan = x.NgungBan
        }).ToList();

        var loiNhap = KiemTraNguoiNhan(nguoiNhan);
        if (loiNhap is not null) return new(model, loiNhap);
        if (rows.Count == 0) return new(model, "Giỏ hàng đang trống.");
        if (rows.Count > ToiDaSoLoai)
            return new(model, $"Mỗi lần đặt hỗ trợ tối đa {ToiDaSoLoai} loại linh kiện. Hãy giảm số loại trong giỏ.");
        var loiGio = model.GioHang.Dongs.FirstOrDefault(x => x.CanhBao is not null);
        if (loiGio is not null) return new(model, $"{loiGio.TenLinhKien}: {loiGio.CanhBao}");
        if (rows.Any(x => x.RowVersion.Length == 0))
            return new(model, "Thiếu RowVersion giỏ hàng. Cần kiểm tra migration giỏ của Tuần 03.");
        // Dữ liệu tiền phù hợp với precision(18,0); không tự làm tròn tiền âm/lẻ.
        decimal tong = 0;
        foreach (var row in rows)
        {
            if (!TienHopLe(row.Gia, row.SoLuong, ref tong))
                return new(model, "Tổng tiền không hợp lệ hoặc vượt giới hạn lưu trữ. Hãy kiểm tra giá và số lượng.");
        }

        var phieu = new PhieuDatHang
        {
            NguoiDungId = userId,
            MaYeuCau = Guid.NewGuid(),
            HetHanUtc = DateTime.UtcNow.AddMinutes(20),
            NguoiNhan = nguoiNhan,
            Dongs = rows.Select(x => new DongPhieu
            {
                GioHangItemId = x.Id, LinhKienId = x.LinhKienId,
                SoLuong = x.SoLuong, GiaDaXem = x.Gia, RowVersion = x.RowVersion
            }).ToList()
        };
        model.PhieuXacNhan = _protector.Protect(JsonSerializer.Serialize(phieu));
        model.HetHanXacNhanUtc = phieu.HetHanUtc;
        return new(model, null);
    }

    public async Task<DatHangKetQua> XacNhanAsync(
        string userId, string token, CancellationToken ct = default)
    {
        var phieu = DocPhieu(userId, token);
        if (phieu is null)
            return new(null, "Phiếu xác nhận không hợp lệ, đã bị sửa hoặc không thuộc tài khoản này. Hãy xem lại giỏ.");

        try
        {
            // Toàn bộ transaction là một đơn vị retry. Dùng cùng MaYeuCau để
            // xử lý trường hợp DB đã commit nhưng ứng dụng mất phản hồi.
            var strategy = _db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                ct.ThrowIfCancellationRequested();
                // Action đặt hàng không có thay đổi ngoài transaction này.
                _db.ChangeTracker.Clear();
                return await GhiDonMotLanAsync(userId, phieu, ct);
            });
        }
        catch (DatHangThayDoiException ex)
        {
            _db.ChangeTracker.Clear();
            return new(null, ex.Message, NguoiNhan: phieu.NguoiNhan);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _db.ChangeTracker.Clear();
            _logger.LogWarning(ex, "Xung đột giỏ khi đặt; yêu cầu {MaYeuCau}.", phieu.MaYeuCau);
            return new(null, "Giỏ vừa thay đổi. Giao dịch chưa hoàn tất; vui lòng xem lại giỏ và xác nhận lại.",
                NguoiNhan: phieu.NguoiNhan);
        }
        catch (Exception ex) when (ex is DbUpdateException or DbException or RetryLimitExceededException)
        {
            _db.ChangeTracker.Clear();
            // Không khẳng định giao dịch thất bại khi mất kết nối ngay tại COMMIT.
            // Không phát phiếu mới: lần gửi lại vẫn dùng chính khóa chống trùng.
            _logger.LogError(ex, "Chưa xác định kết quả đặt hàng; yêu cầu {MaYeuCau}.", phieu.MaYeuCau);
            return new(null,
                "Chưa xác nhận được kết quả do cơ sở dữ liệu bận hoặc mất kết nối. " +
                "Bấm Kiểm tra / gửi lại cùng yêu cầu. Nếu đã lưu, hệ thống sẽ mở đúng đơn cũ, không tạo thêm.",
                ThuLaiCungYeuCau: true, NguoiNhan: phieu.NguoiNhan);
        }
    }

    private async Task<DatHangKetQua> GhiDonMotLanAsync(
        string userId, PhieuDatHang phieu, CancellationToken ct)
    {
        // Phải kiểm tra yêu cầu đã lưu TRƯỚC khi xét hạn: gửi lại đơn đã thành
        // công phải mở lại đơn đó, kể cả giỏ đã rỗng hoặc phiếu vừa hết hạn.
        var daCo = await TimDonDaLuuAsync(userId, phieu.MaYeuCau, ct);
        if (daCo.HasValue) return new(daCo);

        if (phieu.HetHanUtc <= DateTime.UtcNow)
            throw new DatHangThayDoiException("Phiếu đã hết hạn 20 phút. Hãy xem lại giá và giỏ để xác nhận lại.");

        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        // Chỉ nâng mức khóa tại giỏ/kho bằng HOLDLOCK; tránh khóa range không
        // cần thiết trên index MaYeuCau khi nhiều khách tạo đơn khác nhau.
        // SQL Server: khóa các dòng/range giỏ của tài khoản đến hết transaction.
        // FromSqlInterpolated tự tham số hóa userId, KHÔNG nối chuỗi SQL.
        var gio = await _db.GioHangItems.FromSqlInterpolated($@"
            SELECT * FROM [dbo].[GioHangItem] WITH (UPDLOCK, HOLDLOCK)
            WHERE [NguoiDungId] = {userId}")
            .AsNoTracking().OrderBy(x => x.LinhKienId).ToListAsync(ct);

        // Yêu cầu thứ hai cùng tài khoản đợi khóa; khi có khóa, kiểm tra lại.
        daCo = await TimDonDaLuuAsync(userId, phieu.MaYeuCau, ct);
        if (daCo.HasValue)
        {
            await tx.CommitAsync(ct);
            return new(daCo);
        }
        if (phieu.HetHanUtc <= DateTime.UtcNow)
            throw new DatHangThayDoiException("Phiếu đã hết hạn. Hãy kiểm tra lại giỏ hàng.");
        if (gio.Count == 0 || gio.Count != phieu.Dongs.Count)
            throw new DatHangThayDoiException("Giỏ đã thay đổi hoặc đã được đặt ở cửa sổ khác. Hãy kiểm tra lại giỏ.");

        var daXem = phieu.Dongs.ToDictionary(x => x.GioHangItemId);
        foreach (var dong in gio)
        {
            if (!daXem.TryGetValue(dong.Id, out var cu)
                || dong.LinhKienId != cu.LinhKienId || dong.SoLuong != cu.SoLuong
                || !dong.RowVersion.SequenceEqual(cu.RowVersion))
                throw new DatHangThayDoiException("Số lượng hoặc nội dung giỏ vừa thay đổi. Hãy xem lại trước khi gửi đơn.");
        }

        // Khóa linh kiện theo thứ tự Id nhất quán, giảm khả năng deadlock giữa
        // các yêu cầu đặt hàng. Không giữ khóa trong lúc khách nhập biểu mẫu.
        var sanPhams = new Dictionary<int, LinhKien>();
        decimal tongTien = 0;
        foreach (var dong in gio.OrderBy(x => x.LinhKienId))
        {
            int id = dong.LinhKienId;
            var sp = await _db.LinhKiens.FromSqlInterpolated($@"
                SELECT * FROM [dbo].[LinhKien] WITH (UPDLOCK, HOLDLOCK)
                WHERE [Id] = {id}")
                .AsNoTracking().SingleOrDefaultAsync(ct);
            if (sp is null || sp.NgungBan)
                throw new DatHangThayDoiException("Có linh kiện đã ngừng bán hoặc không còn tồn tại. Hãy cập nhật giỏ.");
            if (dong.SoLuong <= 0 || sp.SoLuongTon < dong.SoLuong)
                throw new DatHangThayDoiException($"{sp.TenLinhKien}: kho chỉ còn {Math.Max(0, sp.SoLuongTon)}; hãy cập nhật số lượng.");
            if (sp.Gia != daXem[dong.Id].GiaDaXem)
                throw new DatHangThayDoiException($"Giá {sp.TenLinhKien} vừa thay đổi. Hãy xem lại giá mới trước khi đặt.");
            if (!TienHopLe(sp.Gia, dong.SoLuong, ref tongTien))
                throw new DatHangThayDoiException("Giá hoặc tổng tiền không hợp lệ/vượt giới hạn lưu trữ.");
            if (string.IsNullOrWhiteSpace(sp.MaLinhKien) || sp.MaLinhKien.Length > 30
                || string.IsNullOrWhiteSpace(sp.TenLinhKien) || sp.TenLinhKien.Length > 200)
                throw new DatHangThayDoiException("Mã/tên linh kiện không phù hợp dữ liệu chi tiết đơn hàng. Vui lòng liên hệ quản trị.");
            sanPhams.Add(id, sp);
        }

        // Trừ trực tiếp trên SQL bằng điều kiện. Không dùng giá trị tồn cũ
        // ở trình duyệt, không tải tồn rồi ghi đè một con số tính ngoài SQL.
        foreach (var dong in gio.OrderBy(x => x.LinhKienId))
        {
            var sp = sanPhams[dong.LinhKienId];
            int soLuong = dong.SoLuong;
            int affected = await _db.LinhKiens
                .Where(x => x.Id == dong.LinhKienId && !x.NgungBan
                    && x.Gia == sp.Gia && x.SoLuongTon >= soLuong)
                .ExecuteUpdateAsync(setters => setters.SetProperty(
                    x => x.SoLuongTon, x => x.SoLuongTon - soLuong), ct);
            if (affected != 1)
                throw new DatHangThayDoiException("Kho vừa thay đổi. Không có đơn mới được hoàn tất; hãy xem lại giỏ.");
        }

        var nguoiNhan = phieu.NguoiNhan;
        var don = new DonHang
        {
            MaYeuCau = phieu.MaYeuCau,
            NguoiDungId = userId,
            TenNguoiNhan = nguoiNhan.TenNguoiNhan,
            SoDienThoai = nguoiNhan.SoDienThoai,
            DiaChiGiaoHang = nguoiNhan.DiaChiGiaoHang,
            GhiChu = nguoiNhan.GhiChu,
            NgayDatUtc = DateTime.UtcNow,
            TrangThai = TrangThaiDonHang.ChoXacNhan,
            TongTien = tongTien
        };
        foreach (var dong in gio)
        {
            var sp = sanPhams[dong.LinhKienId];
            don.ChiTiets.Add(new ChiTietDonHang
            {
                LinhKienId = sp.Id, MaLinhKien = sp.MaLinhKien,
                TenLinhKien = sp.TenLinhKien, SoLuong = dong.SoLuong,
                DonGia = sp.Gia, ThanhTien = sp.Gia * dong.SoLuong
            });
        }

        _db.DonHangs.Add(don);
        // Chỉ xóa chính các dòng đã đọc/kiểm tra. RowVersion tham gia DELETE.
        _db.GioHangItems.RemoveRange(gio);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return new(don.Id);
        // Nếu ném lỗi trước khi commit, await using dispose transaction để rollback
        // cả ExecuteUpdate (tồn), đơn, chi tiết và xóa giỏ. Không rollback từng bảng.
    }

    public Task<DonHang?> LayDonCuaToiAsync(string userId, int id, CancellationToken ct = default)
        => _db.DonHangs.AsNoTracking().Include(x => x.ChiTiets)
            .SingleOrDefaultAsync(x => x.Id == id && x.NguoiDungId == userId, ct);

    private Task<int?> TimDonDaLuuAsync(string userId, Guid maYeuCau, CancellationToken ct)
        => _db.DonHangs.AsNoTracking()
            .Where(x => x.NguoiDungId == userId && x.MaYeuCau == maYeuCau)
            .Select(x => (int?)x.Id).SingleOrDefaultAsync(ct);

    private PhieuDatHang? DocPhieu(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)
            || token.Length > 131072) return null;
        try
        {
            var p = JsonSerializer.Deserialize<PhieuDatHang>(_protector.Unprotect(token));
            if (p is null || p.NguoiDungId != userId || p.MaYeuCau == Guid.Empty
                || p.NguoiNhan is null || KiemTraNguoiNhan(p.NguoiNhan) is not null
                || p.Dongs is null || p.Dongs.Count == 0 || p.Dongs.Count > ToiDaSoLoai
                || p.Dongs.Any(x => x is null || x.GioHangItemId <= 0 || x.LinhKienId <= 0
                    || x.SoLuong <= 0 || x.GiaDaXem < 0 || x.RowVersion is null || x.RowVersion.Length == 0)
                || p.Dongs.Select(x => x.GioHangItemId).Distinct().Count() != p.Dongs.Count
                || p.Dongs.Select(x => x.LinhKienId).Distinct().Count() != p.Dongs.Count)
                return null;
            return p;
        }
        catch (Exception ex) when (ex is CryptographicException or JsonException or FormatException)
        {
            // Không log phiếu vì có chứa thông tin người nhận đã mã hóa.
            return null;
        }
    }

    private static ThongTinNhanHangInput ChuanHoa(ThongTinNhanHangInput input) => new()
    {
        TenNguoiNhan = input.TenNguoiNhan?.Trim() ?? "",
        SoDienThoai = input.SoDienThoai?.Trim() ?? "",
        DiaChiGiaoHang = input.DiaChiGiaoHang?.Trim() ?? "",
        GhiChu = string.IsNullOrWhiteSpace(input.GhiChu) ? null : input.GhiChu.Trim()
    };

    private static string? KiemTraNguoiNhan(ThongTinNhanHangInput input)
    {
        var errors = new List<ValidationResult>();
        return Validator.TryValidateObject(input, new ValidationContext(input), errors, true)
            ? null : string.Join(" ", errors.Select(x => x.ErrorMessage));
    }

    private static bool TienHopLe(decimal gia, int soLuong, ref decimal tong)
    {
        if (gia < 0 || decimal.Truncate(gia) != gia || soLuong <= 0
            || gia > TienToiDa / soLuong) return false;
        decimal thanhTien = gia * soLuong;
        if (tong > TienToiDa - thanhTien) return false;
        tong += thanhTien;
        return true;
    }

    private sealed class DatHangThayDoiException : Exception
    {
        public DatHangThayDoiException(string message) : base(message) { }
    }

    private sealed class DongXemLai
    {
        public int Id { get; set; }
        public int LinhKienId { get; set; }
        public int SoLuong { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public string Ma { get; set; } = "";
        public string Ten { get; set; } = "";
        public string Slug { get; set; } = "";
        public string? Anh { get; set; }
        public decimal Gia { get; set; }
        public int Ton { get; set; }
        public bool NgungBan { get; set; }
    }

    // Constructor public để System.Text.Json có thể khởi tạo khi Unprotect.
    private sealed class PhieuDatHang
    {
        public PhieuDatHang() { }
        public string NguoiDungId { get; set; } = "";
        public Guid MaYeuCau { get; set; }
        public DateTime HetHanUtc { get; set; }
        public ThongTinNhanHangInput NguoiNhan { get; set; } = new();
        public List<DongPhieu> Dongs { get; set; } = new();
    }

    private sealed class DongPhieu
    {
        public DongPhieu() { }
        public int GioHangItemId { get; set; }
        public int LinhKienId { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaDaXem { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
