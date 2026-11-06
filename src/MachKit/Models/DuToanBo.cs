namespace MachKit.Models;

// Định mức là số linh kiện cần để làm MỘT bộ; tồn là số lượng của cả cửa hàng.
public sealed record LinhKienMau(string Ma, string Ten, int DinhMuc, int Ton, decimal DonGia);
public sealed record DongDuToan(LinhKienMau LinhKien, int SoBo)
{
    public int Can => LinhKien.DinhMuc * SoBo;
    public int Thieu => Math.Max(0, Can - LinhKien.Ton);
    public decimal ThanhTien => Can * LinhKien.DonGia;
}

public sealed class DuToanBo
{
    public required string GiaTriNhap { get; init; }
    public string? Loi { get; init; }
    public required IReadOnlyList<DongDuToan> Dong { get; init; }
    public required int SoBoToiDa { get; init; }
    public decimal TongTien => Dong.Sum(d => d.ThanhTien);
    public bool DuHang => Loi is null && Dong.Count > 0 && Dong.All(d => d.Thieu == 0);
}
