using MachKit.Models;

namespace MachKit.ViewModels;

public class LinhKienChiTietViewModel
{
    public LinhKien SanPham { get; set; } = null!;
    public List<LinhKien> SanPhamLienQuan { get; set; } = new();
    public string? DatasheetHttpUrl { get; set; }
}
