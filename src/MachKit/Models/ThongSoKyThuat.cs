using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class ThongSoKyThuat
{
    public int Id { get; set; }

    public int LinhKienId { get; set; }
    public LinhKien? LinhKien { get; set; }

    [Required, StringLength(100)]
    public string TenThongSo { get; set; } = "";

    [Required, StringLength(200)]
    public string GiaTri { get; set; } = "";

    [StringLength(50)]
    public string? DonVi { get; set; }
}