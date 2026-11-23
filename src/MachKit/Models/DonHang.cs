using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public static class TrangThaiDonHang
{
    public const string ChoXacNhan = "ChoXacNhan";
    public const string DaXacNhan = "DaXacNhan";
    public const string DangGiao = "DangGiao";
    public const string HoanThanh = "HoanThanh";
    public const string DaHuy = "DaHuy";
}

public class DonHang
{
    public int Id { get; set; }

    [Required, StringLength(450)]
    public string NguoiDungId { get; set; } = "";
    public ApplicationUser? NguoiDung { get; set; }

    [Required, StringLength(120)]
    public string TenNguoiNhan { get; set; } = "";

    [Required, StringLength(20)]
    public string SoDienThoai { get; set; } = "";

    [Required, StringLength(300)]
    public string DiaChiGiaoHang { get; set; } = "";

    [StringLength(1000)]
    public string? GhiChu { get; set; }

    public DateTime NgayDatUtc { get; set; } = DateTime.UtcNow;

    [Required, StringLength(30)]
    public string TrangThai { get; set; } = TrangThaiDonHang.ChoXacNhan;

    public decimal TongTien { get; set; }

    public ICollection<ChiTietDonHang> ChiTiets { get; set; }
        = new List<ChiTietDonHang>();
}
