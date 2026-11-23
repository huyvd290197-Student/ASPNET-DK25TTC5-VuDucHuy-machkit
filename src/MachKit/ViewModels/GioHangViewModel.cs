namespace MachKit.ViewModels;

public class GioHangDongViewModel
{
    public int Id { get; set; }
    public int LinhKienId { get; set; }
    public string MaLinhKien { get; set; } = "";
    public string TenLinhKien { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? HinhAnh { get; set; }
    public int SoLuong { get; set; }
    public int TonKho { get; set; }
    public decimal DonGia { get; set; }
    public bool NgungBan { get; set; }
    public decimal ThanhTien => DonGia * SoLuong;

    public string? CanhBao => NgungBan ? "Linh kiện đã ngừng bán. Hãy xóa khỏi giỏ."
        : TonKho <= 0 ? "Linh kiện đã hết hàng."
        : SoLuong > TonKho ? $"Chỉ còn {TonKho}; hãy giảm số lượng hoặc xóa khỏi giỏ."
        : SoLuong <= 0 ? "Số lượng không hợp lệ."
        : DonGia < 0 ? "Giá sản phẩm không hợp lệ; liên hệ quản trị."
        : null;
}

public class GioHangViewModel
{
    public List<GioHangDongViewModel> Dongs { get; set; } = new();
    public decimal TongTien => Dongs.Sum(x => x.ThanhTien);
    public bool CoCanhBao => Dongs.Any(x => x.CanhBao is not null);
}
