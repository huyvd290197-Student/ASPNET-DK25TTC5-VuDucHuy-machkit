using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

// Nhật ký chỉ được thêm cùng transaction đổi trạng thái. Không có chức năng sửa/xóa.
public class LichSuTrangThaiDonHang
{
    public int Id { get; set; }
    public int DonHangId { get; set; }
    public DonHang? DonHang { get; set; }

    public Guid MaThaoTac { get; set; }

    [Required, StringLength(450)]
    public string NguoiThucHienId { get; set; } = "";
    public ApplicationUser? NguoiThucHien { get; set; }

    [Required, StringLength(30)]
    public string TrangThaiCu { get; set; } = "";
    [Required, StringLength(30)]
    public string TrangThaiMoi { get; set; } = "";

    // Ghi chú nội bộ, không trả về trang khách.
    [StringLength(500)]
    public string? GhiChu { get; set; }
    public DateTime ThoiDiemUtc { get; set; } = DateTime.UtcNow;
    public bool DaHoanTon { get; set; }
}
