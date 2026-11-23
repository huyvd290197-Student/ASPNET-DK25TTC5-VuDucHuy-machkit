using System.ComponentModel.DataAnnotations;

namespace MachKit.ViewModels;

// Không nhận giá, tổng tiền, mã tài khoản hoặc số lượng do trình duyệt tự khai báo.
public sealed class XacNhanDatHangInput
{
    [Required(ErrorMessage = "Phiếu xác nhận bị thiếu. Vui lòng xem lại giỏ hàng.")]
    [StringLength(131072, ErrorMessage = "Phiếu xác nhận không hợp lệ.")]
    public string PhieuXacNhan { get; set; } = "";
}

public sealed class DatHangThuLaiViewModel
{
    public string PhieuXacNhan { get; set; } = "";
    public string ThongBao { get; set; } = "";
}
