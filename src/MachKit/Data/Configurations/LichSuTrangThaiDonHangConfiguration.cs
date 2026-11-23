using MachKit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachKit.Data.Configurations;

public sealed class LichSuTrangThaiDonHangConfiguration
    : IEntityTypeConfiguration<LichSuTrangThaiDonHang>
{
    public void Configure(EntityTypeBuilder<LichSuTrangThaiDonHang> e)
    {
        e.ToTable("LichSuTrangThaiDonHang", table => table.HasCheckConstraint(
            "CK_LichSuDonHang_HoanTon",
            "([TrangThaiMoi] = N'DaHuy' AND [DaHoanTon] = 1) OR " +
            "([TrangThaiMoi] <> N'DaHuy' AND [DaHoanTon] = 0)"));

        e.HasOne(x => x.DonHang).WithMany()
            .HasForeignKey(x => x.DonHangId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne(x => x.NguoiThucHien).WithMany()
            .HasForeignKey(x => x.NguoiThucHienId).OnDelete(DeleteBehavior.Restrict);

        e.HasIndex(x => x.MaThaoTac).IsUnique()
            .HasDatabaseName("UX_LichSuDonHang_MaThaoTac");
        // Thêm lớp bảo vệ: một đơn chỉ có tối đa một nhật ký hủy.
        e.HasIndex(x => x.DonHangId).IsUnique()
            .HasFilter("[TrangThaiMoi] = N'DaHuy'")
            .HasDatabaseName("UX_LichSuDonHang_HuyMotLan");
        e.HasIndex(x => new { x.DonHangId, x.ThoiDiemUtc, x.Id });
    }
}
