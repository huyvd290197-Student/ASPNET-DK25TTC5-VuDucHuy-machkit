using MachKit.Models;

namespace MachKit.ViewModels;

public class LinhKienDanhSachViewModel
{
    // Điều kiện tìm kiếm/lọc nhận từ URL
    public string? TuKhoa { get; set; }
    public int? DanhMucId { get; set; }
    public int? ThuongHieuId { get; set; }
    public decimal? GiaTu { get; set; }
    public decimal? GiaDen { get; set; }
    public string SapXep { get; set; } = "ten_az";

    // Dữ liệu dùng để vẽ danh sách và bộ lọc
    public List<LinhKien> LinhKiens { get; set; } = new();
    public List<DanhMuc> DanhMucs { get; set; } = new();
    public List<ThuongHieu> ThuongHieus { get; set; } = new();

    // Thông tin phân trang
    public int Trang { get; set; } = 1;
    public int KichThuocTrang { get; set; } = 6;
    public int TongSo { get; set; }
    public int TongTrang =>
        (int)Math.Ceiling(TongSo / (double)KichThuocTrang);
}
