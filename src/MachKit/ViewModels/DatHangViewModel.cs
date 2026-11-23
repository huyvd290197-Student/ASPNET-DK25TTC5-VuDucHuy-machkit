using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MachKit.ViewModels;

// Chi nhan bon truong nguoi nhan tu form. Khong nhan userId, gia,
// tong tien, so luong hay danh sach san pham do trinh duyet gui len.
public class ThongTinNhanHangInput : IValidatableObject
{
    [Display(Name = "Họ và tên người nhận")]
    [Required(ErrorMessage = "Vui lòng nhập tên người nhận.")]
    [StringLength(120, MinimumLength = 2,
        ErrorMessage = "Tên người nhận phải từ 2 đến 120 ký tự.")]
    public string TenNguoiNhan { get; set; } = "";

    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [StringLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự.")]
    public string SoDienThoai { get; set; } = "";

    [Display(Name = "Địa chỉ giao hàng")]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
    [StringLength(300, MinimumLength = 5,
        ErrorMessage = "Địa chỉ phải từ 5 đến 300 ký tự.")]
    public string DiaChiGiaoHang { get; set; } = "";

    [Display(Name = "Ghi chú")]
    [StringLength(1000, ErrorMessage = "Ghi chú tối đa 1.000 ký tự.")]
    public string? GhiChu { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        string ten = TenNguoiNhan?.Trim() ?? "";
        if (ten.Length < 2)
        {
            yield return new ValidationResult(
                "Tên người nhận phải có ít nhất 2 ký tự, không tính khoảng trắng đầu/cuối.",
                new[] { nameof(TenNguoiNhan) });
        }

        string diaChi = DiaChiGiaoHang?.Trim() ?? "";
        if (diaChi.Length < 5)
        {
            yield return new ValidationResult(
                "Địa chỉ phải có ít nhất 5 ký tự, không tính khoảng trắng đầu/cuối.",
                new[] { nameof(DiaChiGiaoHang) });
        }

        string dienThoai = SoDienThoai?.Trim() ?? "";
        int soChuSo = dienThoai.Count(c => c >= '0' && c <= '9');
        // Kiem tra dinh dang co ban, khong xac minh so nay thuoc ve nguoi nhan.
        if (!Regex.IsMatch(dienThoai, @"^\+?[0-9][0-9 ().-]*$",
                RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100))
            || soChuSo < 9 || soChuSo > 15)
        {
            yield return new ValidationResult(
                "Số điện thoại cần 9–15 chữ số; chỉ chấp nhận dấu + ở đầu, khoảng trắng, dấu chấm, ngoặc và gạch nối.",
                new[] { nameof(SoDienThoai) });
        }
    }
}

// Dùng để hiển thị. POST KiemTra chỉ nhận ThongTinNhanHangInput;
// POST XacNhan chỉ nhận phiếu được máy chủ bảo vệ.
public class DatHangViewModel : ThongTinNhanHangInput
{
    public string? PhieuXacNhan { get; set; }
    public DateTime? HetHanXacNhanUtc { get; set; }

    public GioHangViewModel GioHang { get; set; } = new();
    public decimal TongTien => GioHang.TongTien;
    public bool CoTheXemLai => GioHang.Dongs.Count > 0 && !GioHang.CoCanhBao;
}
