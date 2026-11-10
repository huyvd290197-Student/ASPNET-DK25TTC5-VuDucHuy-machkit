using MachKit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MachKit.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DanhMuc> DanhMucs => Set<DanhMuc>();
    public DbSet<ThuongHieu> ThuongHieus => Set<ThuongHieu>();
    public DbSet<LinhKien> LinhKiens => Set<LinhKien>();
    public DbSet<ThongSoKyThuat> ThongSoKyThuats => Set<ThongSoKyThuat>();
    public DbSet<BoThucHanh> BoThucHanhs => Set<BoThucHanh>();
    public DbSet<ThanhPhanBo> ThanhPhanBos => Set<ThanhPhanBo>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<DanhMuc>().ToTable("DanhMuc");
        b.Entity<ThuongHieu>().ToTable("ThuongHieu");
        b.Entity<LinhKien>().ToTable("LinhKien");
        b.Entity<ThongSoKyThuat>().ToTable("ThongSoKyThuat");
        b.Entity<BoThucHanh>().ToTable("BoThucHanh");
        b.Entity<ThanhPhanBo>().ToTable("ThanhPhanBo");

        b.Entity<DanhMuc>()
            .HasOne(x => x.DanhMucCha)
            .WithMany(x => x.DanhMucCons)
            .HasForeignKey(x => x.DanhMucChaId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<LinhKien>().HasIndex(x => x.MaLinhKien).IsUnique();
        b.Entity<BoThucHanh>().HasIndex(x => x.MaBo).IsUnique();
        b.Entity<LinhKien>().Property(x => x.Gia).HasPrecision(18, 0);

        b.Entity<ThanhPhanBo>()
            .HasKey(x => new { x.BoThucHanhId, x.LinhKienId });

        b.Entity<ThanhPhanBo>()
            .HasOne(x => x.BoThucHanh)
            .WithMany(x => x.ThanhPhans)
            .HasForeignKey(x => x.BoThucHanhId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<ThanhPhanBo>()
            .HasOne(x => x.LinhKien)
            .WithMany(x => x.ThanhPhanBos)
            .HasForeignKey(x => x.LinhKienId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<LinhKien>()
            .HasOne(x => x.DanhMuc)
            .WithMany(x => x.LinhKiens)
            .HasForeignKey(x => x.DanhMucId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<LinhKien>()
            .HasOne(x => x.ThuongHieu)
            .WithMany(x => x.LinhKiens)
            .HasForeignKey(x => x.ThuongHieuId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
