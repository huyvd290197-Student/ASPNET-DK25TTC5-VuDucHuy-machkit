using MachKit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachKit.Data.Configurations;

public class GioHangItemConfiguration : IEntityTypeConfiguration<GioHangItem>
{
    public void Configure(EntityTypeBuilder<GioHangItem> entity)
    {
        entity.ToTable("GioHangItem", table =>
            table.HasCheckConstraint("CK_GioHangItem_SoLuong", "[SoLuong] > 0"));

        entity.HasKey(x => x.Id);
        entity.Property(x => x.NguoiDungId).IsRequired().HasMaxLength(450);

        // Một tài khoản chỉ có một dòng cho mỗi linh kiện.
        entity.HasIndex(x => new { x.NguoiDungId, x.LinhKienId }).IsUnique();

        entity.HasOne(x => x.NguoiDung)
            .WithMany()
            .HasForeignKey(x => x.NguoiDungId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.LinhKien)
            .WithMany()
            .HasForeignKey(x => x.LinhKienId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
