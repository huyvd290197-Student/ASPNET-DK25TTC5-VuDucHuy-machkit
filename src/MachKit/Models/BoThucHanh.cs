using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class BoThucHanh
{
    public int Id { get; set; }

    [Required, StringLength(30)]
    public string MaBo { get; set; } = "";

    [Required, StringLength(200)]
    public string TenBo { get; set; } = "";

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public bool KichHoat { get; set; } = true;

    public ICollection<ThanhPhanBo> ThanhPhans { get; set; }
        = new List<ThanhPhanBo>();
}