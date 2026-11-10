using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class DanhMuc
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string TenDanhMuc { get; set; } = "";

    [Required, StringLength(120)]
    public string Slug { get; set; } = "";

    public int ThuTu { get; set; }

    public ICollection<LinhKien> LinhKiens { get; set; }
        = new List<LinhKien>();
}