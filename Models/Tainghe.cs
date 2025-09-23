using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Tainghe")]
public partial class Tainghe
{
    [Key]
    [Column("MaTN")]
    [StringLength(10)]
    public string MaTn { get; set; } = null!;

    [Column("TenTN")]
    [StringLength(100)]
    public string TenTn { get; set; } = null!;

    [Column("HangTN")]
    [StringLength(50)]
    public string HangTn { get; set; } = null!;

    [Column("GiaTN")]
    public int GiaTn { get; set; }

    [Column("LoaiTN")]
    [StringLength(50)]
    public string LoaiTn { get; set; } = null!;

    [Column("KieuketnoiTN")]
    [StringLength(50)]
    public string? KieuketnoiTn { get; set; }

    [Column("PinTN")]
    [StringLength(50)]
    public string? PinTn { get; set; }

    [Column("TrongluongTN")]
    [StringLength(50)]
    public string? TrongluongTn { get; set; }

    [Column("XuatxuTN")]
    [StringLength(50)]
    public string XuatxuTn { get; set; } = null!;

    [Column("NgaysanxuatTN")]
    public DateOnly? NgaysanxuatTn { get; set; }

    [Column("TinhtrangTN")]
    [StringLength(20)]
    public string TinhtrangTn { get; set; } = null!;

    [Column("MaNCC")]
    [StringLength(10)]
    public string? MaNcc { get; set; }

    [Column("MaPLSP")]
    [StringLength(10)]
    public string? MaPlsp { get; set; }

    [Column("MaHH")]
    [StringLength(10)]
    public string? MaHh { get; set; }

    [ForeignKey("MaPlsp")]
    [InverseProperty("Tainghes")]
    public virtual PhanloaiSp? MaPlspNavigation { get; set; }

    [ForeignKey("MaHh")]
    [InverseProperty("Tainghes")]
    public virtual Hanghoa? MaHhNavigation { get; set; }
}
