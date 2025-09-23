using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Banphim")]
public partial class Banphim
{
    [Key]
    [Column("MaBP")]
    [StringLength(10)]
    public string MaBp { get; set; } = null!;

    [Column("TenBP")]
    [StringLength(100)]
    public string TenBp { get; set; } = null!;

    [Column("HangBP")]
    [StringLength(50)]
    public string HangBp { get; set; } = null!;

    [Column("GiaBP")]
    public int GiaBp { get; set; }

    [Column("LoaiBP")]
    [StringLength(50)]
    public string LoaiBp { get; set; } = null!;

    [Column("KieuketnoiBP")]
    [StringLength(50)]
    public string KieuketnoiBp { get; set; } = null!;

    [Column("DenledBP")]
    public bool DenledBp { get; set; }

    [Column("XuatxuBP")]
    [StringLength(50)]
    public string XuatxuBp { get; set; } = null!;

    [Column("NgaysanxuatBP")]
    public DateOnly? NgaysanxuatBp { get; set; }

    [Column("TinhtrangBP")]
    [StringLength(20)]
    public string TinhtrangBp { get; set; } = null!;

    [Column("MaNCC")]
    [StringLength(10)]
    public string? MaNcc { get; set; }

    [Column("MaPLSP")]
    [StringLength(10)]
    public string? MaPlsp { get; set; }

    [Column("MaHH")]
    [StringLength(10)]
    public string? MaHh { get; set; }

    [ForeignKey("MaHh")]
    [InverseProperty("Banphims")]
    public virtual Hanghoa? MaHhNavigation { get; set; }

    [ForeignKey("MaPlsp")]
    [InverseProperty("Banphims")]
    public virtual PhanloaiSp? MaPlspNavigation { get; set; }
}
