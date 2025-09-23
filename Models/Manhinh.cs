using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Manhinh")]
public partial class Manhinh
{
    [Key]
    [Column("MaMH")]
    [StringLength(10)]
    public string MaMh { get; set; } = null!;

    [Column("TenMH")]
    [StringLength(100)]
    public string TenMh { get; set; } = null!;

    [Column("HangMH")]
    [StringLength(50)]
    public string HangMh { get; set; } = null!;

    [Column("GiaMH")]
    public int GiaMh { get; set; }

    [Column("KichthuocMH")]
    [StringLength(50)]
    public string KichthuocMh { get; set; } = null!;

    [Column("TansoMH")]
    [StringLength(20)]
    public string? TansoMh { get; set; }

    [Column("DoPhanGiaiMH")]
    [StringLength(50)]
    public string? DoPhanGiaiMh { get; set; }

    [Column("XuatxuMH")]
    [StringLength(50)]
    public string XuatxuMh { get; set; } = null!;

    [Column("NgaysanxuatMH")]
    public DateOnly? NgaysanxuatMh { get; set; }

    [Column("TinhtrangMH")]
    [StringLength(20)]
    public string TinhtrangMh { get; set; } = null!;

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
    [InverseProperty("Manhinhs")]
    public virtual Hanghoa? MaHhNavigation { get; set; }

    [ForeignKey("MaPlsp")]
    [InverseProperty("Manhinhs")]
    public virtual PhanloaiSp? MaPlspNavigation { get; set; }
}
