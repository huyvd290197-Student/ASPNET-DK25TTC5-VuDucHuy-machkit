using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class LinhKien
{
    public int Id { get; set; }

    [Required, StringLength(30)]
    public string MaLinhKien { get; set; } = "";

    [Required, StringLength(200)]
    public string TenLinhKien { get; set; } = "";

    [Required, StringLength(220)]
    public string Slug { get; set; } = "";

    public string? MoTa { get; set; }

    public decimal Gia { get; set; }

    public int SoLuongTon { get; set; }

    public int MucTonToiThieu { get; set; } = 5;

    public int DanhMucId { get; set; }
    public DanhMuc? DanhMuc { get; set; }

    public int ThuongHieuId { get; set; }
    public ThuongHieu? ThuongHieu { get; set; }

    public string? HinhAnh { get; set; }

    public string? DatasheetUrl { get; set; }

    public bool NgungBan { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public ICollection<ThongSoKyThuat> ThongSos { get; set; }
        = new List<ThongSoKyThuat>();

    public ICollection<ThanhPhanBo> ThanhPhanBos { get; set; }
        = new List<ThanhPhanBo>();
}