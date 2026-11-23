using System.ComponentModel.DataAnnotations;

namespace MachKit.Models;

// Một dòng là một linh kiện trong giỏ của một tài khoản.
// Không phải đơn hàng; chưa trừ hoặc giữ tồn kho.
public class GioHangItem
{
    public int Id { get; set; }

    [Required, StringLength(450)]
    public string NguoiDungId { get; set; } = "";
    public ApplicationUser? NguoiDung { get; set; }

    public int LinhKienId { get; set; }
    public LinhKien? LinhKien { get; set; }

    [Range(1, int.MaxValue)]
    public int SoLuong { get; set; }

    public DateTime CapNhatUtc { get; set; } = DateTime.UtcNow;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
