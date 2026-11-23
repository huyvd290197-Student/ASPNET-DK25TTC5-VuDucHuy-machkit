using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class ChiTietDonHang
{
    public int Id { get; set; }

    public int DonHangId { get; set; }
    public DonHang? DonHang { get; set; }

    public int LinhKienId { get; set; }
    public LinhKien? LinhKien { get; set; }

    // Lưu thông tin tại thời điểm đặt để giữ lịch sử
    [Required, StringLength(30)]
    public string MaLinhKien { get; set; } = "";

    [Required, StringLength(200)]
    public string TenLinhKien { get; set; } = "";

    [Range(1, int.MaxValue)]
    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }
    public decimal ThanhTien { get; set; }
}
