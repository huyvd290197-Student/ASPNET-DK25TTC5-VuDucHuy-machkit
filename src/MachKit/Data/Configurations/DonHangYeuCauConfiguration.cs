using MachKit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachKit.Data.Configurations;

// Chỉ bổ sung cơ chế chống gửi trùng. Giữ nguyên cấu hình đơn hàng Chặng 4A.
public sealed class DonHangYeuCauConfiguration : IEntityTypeConfiguration<DonHang>
{
    public void Configure(EntityTypeBuilder<DonHang> entity)
    {
        entity.Property(x => x.MaYeuCau).IsRequired(false);
        entity.HasIndex(x => x.MaYeuCau)
            .HasDatabaseName("UX_DonHang_MaYeuCau")
            .IsUnique()
            .HasFilter("[MaYeuCau] IS NOT NULL");
    }
}
