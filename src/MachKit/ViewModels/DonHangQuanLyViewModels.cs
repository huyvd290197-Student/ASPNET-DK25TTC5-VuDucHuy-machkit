using System.ComponentModel.DataAnnotations;
using MachKit.Helpers;
using MachKit.Models;

namespace MachKit.ViewModels;

public sealed class DonHangTomTatViewModel
{
    public int Id { get; set; }
    public DateTime NgayDatUtc { get; set; }
    public string TenNguoiNhan { get; set; } = "";
    public string SoDienThoai { get; set; } = "";
    public string TrangThai { get; set; } = "";
    public decimal TongTien { get; set; }
    public int SoLoai { get; set; }
}

public sealed class DonHangDanhSachViewModel
{
    public string? TuKhoa { get; set; }
    public string? TrangThai { get; set; }
    public int Trang { get; set; } = 1;
    public int KichThuocTrang { get; set; } = 10;
    public int TongSo { get; set; }
    public int TongTrang => Math.Max(1,
        (int)Math.Ceiling(TongSo / (double)KichThuocTrang));
    public List<DonHangTomTatViewModel> Dons { get; set; } = new();
}

public sealed class NhatKyDonHangViewModel
{
    public DateTime ThoiDiemUtc { get; set; }
    public string TrangThaiCu { get; set; } = "";
    public string TrangThaiMoi { get; set; } = "";
    public bool DaHoanTon { get; set; }
    // Chỉ nạp ở truy vấn quản trị; không nạp các trường này cho khách.
    public string? NguoiThucHien { get; set; }
    public string? GhiChu { get; set; }
}

public sealed class DonHangChiTietViewModel
{
    public DonHang Don { get; set; } = new();
    public bool LaQuanTri { get; set; }
    public List<NhatKyDonHangViewModel> LichSu { get; set; } = new();
    public CapNhatTrangThaiDonHangInput CapNhat { get; set; } = new();
    public bool DonTu4C => Don.MaYeuCau.HasValue && Don.MaYeuCau.Value != Guid.Empty;
}

// Không bind trực tiếp DonHang: không nhận giá, tồn, địa chỉ, userId từ POST quản trị.
public sealed class CapNhatTrangThaiDonHangInput : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Mã đơn không hợp lệ.")]
    public int Id { get; set; }
    public Guid MaThaoTac { get; set; }
    [Required(ErrorMessage = "Thiếu trạng thái đã xem."), StringLength(30)]
    public string TrangThaiCu { get; set; } = "";
    [Required(ErrorMessage = "Hãy chọn trạng thái mới."), StringLength(30)]
    public string TrangThaiMoi { get; set; } = "";
    [StringLength(500, ErrorMessage = "Ghi chú tối đa 500 ký tự.")]
    public string? GhiChu { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (MaThaoTac == Guid.Empty)
            yield return new ValidationResult("Thiếu mã thao tác. Hãy tải lại trang.");
        if (!DonHang4DHelper.DuocChuyen(TrangThaiCu, TrangThaiMoi))
            yield return new ValidationResult("Không cho phép chuyển trạng thái theo hướng đã chọn.");
        if (TrangThaiMoi == TrangThaiDonHang.DaHuy && (GhiChu?.Trim().Length ?? 0) < 5)
            yield return new ValidationResult("Khi hủy đơn, nhập lý do ít nhất 5 ký tự.",
                new[] { nameof(GhiChu) });
    }
}

public sealed class CapNhatDonHangThuLaiViewModel
{
    public CapNhatTrangThaiDonHangInput YeuCau { get; set; } = new();
    public string ThongBao { get; set; } = "";
}
