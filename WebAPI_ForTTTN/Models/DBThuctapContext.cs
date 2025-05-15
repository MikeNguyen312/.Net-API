using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAPI_ForTTTN.Models
{
    public partial class DBThuctapContext : DbContext
    {
        public DBThuctapContext()
        {
        }

        public DBThuctapContext(DbContextOptions<DBThuctapContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; } = null!;
        public virtual DbSet<PhieuKho> PhieuKhos { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<SanPhamPhieuKho> SanPhamPhieuKhos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=44.204.183.65,1433;Database=DBThuctap;User Id=demo;Password=aA1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => new { e.IdDonHang, e.IdSanPham })
                    .HasName("PK__ChiTietD__AFA0CC00483FE7BD");

                entity.ToTable("ChiTietDonHang");

                entity.Property(e => e.IdDonHang)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_DonHang");

                entity.Property(e => e.IdSanPham)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_SanPham");

                entity.HasOne(d => d.IdDonHangNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.IdDonHang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDo__ID_Do__5070F446");

                entity.HasOne(d => d.IdSanPhamNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.IdSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietDonHang_ID_SanPham");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.IdDonHang)
                    .HasName("PK__DonHang__99B72639199FE2CC");

                entity.ToTable("DonHang");

                entity.Property(e => e.IdDonHang)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_DonHang");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.IdKhachHang)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_KhachHang");

                entity.Property(e => e.NgayDatHang).HasColumnType("date");

                entity.Property(e => e.PhuongThuc).HasMaxLength(100);

                entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrangthaiDh)
                    .HasMaxLength(150)
                    .HasColumnName("trangthaiDH");

                entity.HasOne(d => d.IdKhachHangNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.IdKhachHang)
                    .HasConstraintName("FK__DonHang__ID_Khac__4BAC3F29");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.IdKhachHang)
                    .HasName("PK__KhachHan__263C4E85F8109121");

                entity.ToTable("KhachHang");

                entity.Property(e => e.IdKhachHang)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_KhachHang");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.HoTen).HasMaxLength(255);

                entity.Property(e => e.Passwords)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RoleWeb)
                    .HasMaxLength(150)
                    .HasColumnName("roleWeb");

                entity.Property(e => e.SoDienThoai).HasMaxLength(20);

                entity.Property(e => e.Trangthai)
                    .HasMaxLength(150)
                    .HasColumnName("trangthai");
            });

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.IdKhuyenMai)
                    .HasName("PK__KhuyenMa__E93749B42FE79697");

                entity.ToTable("KhuyenMai");

                entity.Property(e => e.IdKhuyenMai)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ID_KhuyenMai");

                entity.Property(e => e.PhanTramKm).HasColumnName("PhanTramKM");

                entity.HasMany(d => d.IdSanPhams)
                    .WithMany(p => p.IdKhuyenMais)
                    .UsingEntity<Dictionary<string, object>>(
                        "KhuyenMaiSanPham",
                        l => l.HasOne<SanPham>().WithMany().HasForeignKey("IdSanPham").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__KhuyenMai__ID_Sa__17036CC0"),
                        r => r.HasOne<KhuyenMai>().WithMany().HasForeignKey("IdKhuyenMai").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__KhuyenMai__ID_Kh__160F4887"),
                        j =>
                        {
                            j.HasKey("IdKhuyenMai", "IdSanPham").HasName("PK__KhuyenMa__DF20A38DC26909DC");

                            j.ToTable("KhuyenMaiSanPham");

                            j.IndexerProperty<string>("IdKhuyenMai").HasMaxLength(255).IsUnicode(false).HasColumnName("ID_KhuyenMai");

                            j.IndexerProperty<string>("IdSanPham").HasMaxLength(150).IsUnicode(false).HasColumnName("ID_SanPham");
                        });
            });

            modelBuilder.Entity<PhieuKho>(entity =>
            {
                entity.HasKey(e => e.IdPhieuKho)
                    .HasName("PK__PhieuKho__FEBF2F1A5E1C4AB2");

                entity.ToTable("PhieuKho");

                entity.Property(e => e.IdPhieuKho)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_PhieuKho");

                entity.Property(e => e.NgayLap).HasColumnType("date");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.IdSanPham);

                entity.ToTable("SanPham");

                entity.Property(e => e.IdSanPham)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_SanPham");

                entity.Property(e => e.Hang).HasMaxLength(255);

                entity.Property(e => e.Loai).HasMaxLength(100);

                entity.Property(e => e.Size).HasMaxLength(255);

                entity.Property(e => e.Ten).HasMaxLength(255);

                entity.Property(e => e.ThongTin).HasMaxLength(500);
            });

            modelBuilder.Entity<SanPhamPhieuKho>(entity =>
            {
                entity.HasKey(e => new { e.IdPhieuKho, e.IdSanPham })
                    .HasName("PK__SanPham___C8A8C523DE6185F2");

                entity.ToTable("SanPham_PhieuKho");

                entity.Property(e => e.IdPhieuKho)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_PhieuKho");

                entity.Property(e => e.IdSanPham)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_SanPham");

                entity.HasOne(d => d.IdPhieuKhoNavigation)
                    .WithMany(p => p.SanPhamPhieuKhos)
                    .HasForeignKey(d => d.IdPhieuKho)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SanPham_P__ID_Ph__5629CD9C");

                entity.HasOne(d => d.IdSanPhamNavigation)
                    .WithMany(p => p.SanPhamPhieuKhos)
                    .HasForeignKey(d => d.IdSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SanPham_PhieuKho_ID_SanPham");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
