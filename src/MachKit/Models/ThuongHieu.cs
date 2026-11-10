using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class ThuongHieu
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string TenThuongHieu { get; set; } = "";

    [StringLength(100)]
    public string? QuocGia { get; set; }

    public ICollection<LinhKien> LinhKiens { get; set; } = new List<LinhKien>();
}
