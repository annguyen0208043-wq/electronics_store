using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Laptop")]
[Index("TenLt", Name = "UQ__Laptop__4CF9A468F04DF093", IsUnique = true)]
public partial class Laptop
{
    [Key]
    [Column("MaLT")]
    [StringLength(10)]
    public string MaLt { get; set; } = null!;

    [Column("TenLT")]
    [StringLength(100)]
    public string TenLt { get; set; } = null!;

    [Column("HangLT")]
    [StringLength(40)]
    public string HangLt { get; set; } = null!;

    [Column("GiaLT")]
    public int GiaLt { get; set; }

    [Column("KichthuocLT")]
    [StringLength(50)]
    public string KichthuocLt { get; set; } = null!;

    [Column("RamLT")]
    [StringLength(20)]
    public string RamLt { get; set; } = null!;

    [Column("OcungLT")]
    [StringLength(20)]
    public string OcungLt { get; set; } = null!;

    [Column("XuatxuLT")]
    [StringLength(50)]
    public string XuatxuLt { get; set; } = null!;

    [Column("NgaysanxuatLT")]
    public DateOnly? NgaysanxuatLt { get; set; }

    [Column("TinhtrangLT")]
    [StringLength(20)]
    public string TinhtrangLt { get; set; } = null!;

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
    [InverseProperty("Laptops")]
    public virtual Hanghoa? MaHhNavigation { get; set; }

    [ForeignKey("MaPlsp")]
    [InverseProperty("Laptops")]
    public virtual PhanloaiSp? MaPlspNavigation { get; set; }
}
