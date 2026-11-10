using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

public class ThanhPhanBo
{
    public int BoThucHanhId { get; set; }
    public BoThucHanh? BoThucHanh { get; set; }

    public int LinhKienId { get; set; }
    public LinhKien? LinhKien { get; set; }

    [Range(1, 999)]
    public int SoLuong { get; set; }
}