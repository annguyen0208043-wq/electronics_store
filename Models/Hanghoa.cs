using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Hanghoa")]
public partial class Hanghoa
{
    [Key]
    [Column("MaHH")]
    [StringLength(10)]
    public string MaHh { get; set; } = null!;

    [Column("TenHH")]
    [StringLength(100)]
    public string TenHh { get; set; } = null!;

    [Column("GiaHH")]
    public int GiaHh { get; set; }

    public int Soluongton { get; set; }

    [StringLength(255)]
    public string? MoTa { get; set; }

    [Column("LoaiHH")]
    [StringLength(50)]
    public string LoaiHh { get; set; } = null!;

    [Column("MaNCC")]
    [StringLength(10)]
    public string? MaNcc { get; set; }

    [Column("NgayTao")]
    public DateTime? NgayTao { get; set; }

    [Column("NgayXoa")]
    public DateTime? NgayXoa { get; set; }

    public virtual ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; } = new List<HinhAnhSanPham>();

    [InverseProperty("MaHhNavigation")]
    public virtual ICollection<Banphim> Banphims { get; set; } = new List<Banphim>();

    [InverseProperty("MaHhNavigation")]
    public virtual ICollection<ChitietHd> ChitietHds { get; set; } = new List<ChitietHd>();

    [InverseProperty("MaHhNavigation")]
    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();

    [InverseProperty("MaHhNavigation")]
    public virtual ICollection<Loa> Loas { get; set; } = new List<Loa>();

    [ForeignKey("MaNcc")]
    [InverseProperty("Hanghoas")]
    public virtual Nhacungcap? MaNccNavigation { get; set; }

    [InverseProperty("MaHhNavigation")]
    public virtual ICollection<Manhinh> Manhinhs { get; set; } = new List<Manhinh>();

    [InverseProperty("MaHhNavigation")]
    public virtual ICollection<Tainghe> Tainghes { get; set; } = new List<Tainghe>();
}
