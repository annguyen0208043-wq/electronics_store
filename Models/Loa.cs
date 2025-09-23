using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Loa")]
public partial class Loa
{
    [Key]
    [StringLength(10)]
    public string MaLoa { get; set; } = null!;

    [StringLength(100)]
    public string TenLoa { get; set; } = null!;

    [StringLength(50)]
    public string HangLoa { get; set; } = null!;

    public int GiaLoa { get; set; }

    [StringLength(50)]
    public string? CongsuatLoa { get; set; }

    [StringLength(50)]
    public string? KieuketnoiLoa { get; set; }

    [StringLength(50)]
    public string XuatxuLoa { get; set; } = null!;

    public DateOnly? NgaysanxuatLoa { get; set; }

    [StringLength(20)]
    public string TinhtrangLoa { get; set; } = null!;

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
    [InverseProperty("Loas")]
    public virtual Hanghoa? MaHhNavigation { get; set; }

    [ForeignKey("MaPlsp")]
    [InverseProperty("Loas")]
    public virtual PhanloaiSp? MaPlspNavigation { get; set; }
}
