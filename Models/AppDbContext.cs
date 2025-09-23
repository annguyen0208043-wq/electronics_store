using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechShop.ViewModels;
namespace TechShop.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Banphim> Banphims { get; set; }

    public virtual DbSet<ChitietHd> ChitietHds { get; set; }

    public virtual DbSet<Hanghoa> Hanghoas { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Laptop> Laptops { get; set; }

    public virtual DbSet<Loa> Loas { get; set; }

    public virtual DbSet<Manhinh> Manhinhs { get; set; }

    public virtual DbSet<Nhacungcap> Nhacungcaps { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<PhanloaiSp> PhanloaiSps { get; set; }

    public virtual DbSet<Tainghe> Tainghes { get; set; }
    public virtual DbSet<ProductViewModel> Products { get; set; }
    public virtual DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=VAN_AN\\MSSQLSERVER12;Initial Catalog=DOTNET;User ID=sa;Password=an123456@;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False\n");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__Accounts__27250070C97282C8");

            entity.Property(e => e.Gmail).HasDefaultValue("");
        });

        modelBuilder.Entity<Banphim>(entity =>
        {
            entity.HasKey(e => e.MaBp).HasName("PK__Banphim__272475AB37053E7E");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Banphims).HasConstraintName("FK__Banphim__MaHH__18EBB532");

            entity.HasOne(d => d.MaPlspNavigation).WithMany(p => p.Banphims).HasConstraintName("FK__Banphim__MaPLSP__70DDC3D8");
        });

        modelBuilder.Entity<ChitietHd>(entity =>
        {
            entity.HasKey(e => e.MaCthd).HasName("PK__ChitietH__1E4FA771E41E6CA7");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChitietHds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChitietHD__MaHD__2B0A656D");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.ChitietHds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChitietHD__MaHH__2BFE89A6");
        });

        modelBuilder.Entity<Hanghoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK__Hanghoa__2725A6E45E5FAD4F");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.Hanghoas).HasConstraintName("FK__Hanghoa__MaNCC__160F4887");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__Hoadon__2725A6E0A90BC80A");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Hoadons).HasConstraintName("FK__Hoadon__MaKH__6754599E");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Hoadons).HasConstraintName("FK__Hoadon__MaNV__68487DD7");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__Khachhan__2725CF1E72F421B0");

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.Khachhangs).HasConstraintName("FK__Khachhang__MaTK__4D94879B");
        });

        modelBuilder.Entity<Laptop>(entity =>
        {
            entity.HasKey(e => e.MaLt).HasName("PK__Laptop__2725C773763C2B6D");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Laptops).HasConstraintName("FK__Laptop__MaHH__17036CC0");

            entity.HasOne(d => d.MaPlspNavigation).WithMany(p => p.Laptops).HasConstraintName("FK__Laptop__MaPLSP__6EF57B66");
        });

        modelBuilder.Entity<Loa>(entity =>
        {
            entity.HasKey(e => e.MaLoa).HasName("PK__Loa__3B98D240FBD17876");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Loas).HasConstraintName("FK__Loa__MaHH__17F790F9");

            entity.HasOne(d => d.MaPlspNavigation).WithMany(p => p.Loas).HasConstraintName("FK__Loa__MaPLSP__71D1E811");
        });

        modelBuilder.Entity<Manhinh>(entity =>
        {
            entity.HasKey(e => e.MaMh).HasName("PK__Manhinh__2725DFD969530BD5");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Manhinhs).HasConstraintName("FK__Manhinh__MaHH__19DFD96B");

            entity.HasOne(d => d.MaPlspNavigation).WithMany(p => p.Manhinhs).HasConstraintName("FK_Manhinh_PhanloaiSP");
        });

        modelBuilder.Entity<Nhacungcap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__Nhacungc__3A185DEBBBD7F3FE");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__Nhanvien__2725D70A04C9B38A");

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.Nhanviens).HasConstraintName("FK__Nhanvien__MaTK__5165187F");
        });

        modelBuilder.Entity<PhanloaiSp>(entity =>
        {
            entity.HasKey(e => e.MaPlsp).HasName("PK__Phanloai__BD3992AFC28AFCE5");
        });

        modelBuilder.Entity<Tainghe>(entity =>
        {
            entity.HasKey(e => e.MaTn).HasName("PK__Tainghe__27250073255F3EB0");

            entity.HasOne(t => t.MaHhNavigation)
                  .WithMany(h => h.Tainghes)
                  .HasForeignKey(t => t.MaHh)
                  .HasConstraintName("FK_Tainghe_Hanghoa");

            entity.HasOne(t => t.MaPlspNavigation)
                  .WithMany(p => p.Tainghes)
                  .HasConstraintName("FK__Tainghe__MaPLSP__6FE99F9F");
        });

        modelBuilder.Entity<HinhAnhSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HinhAnhSanPham__3214EC07A8B5A3C3"); // Đặt tên khóa chính
            entity.Property(e => e.MaHh).IsRequired(); // MaHh là bắt buộc
            entity.Property(e => e.DuongDan).IsRequired(); // DuongDan là bắt buộc

            entity.HasOne(h => h.Hanghoa)
                  .WithMany(h => h.HinhAnhSanPhams)
                  .HasForeignKey(h => h.MaHh)
                  .HasConstraintName("FK_HinhAnhSanPham_Hanghoa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
