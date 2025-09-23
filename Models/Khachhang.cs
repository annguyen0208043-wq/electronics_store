using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Khachhang")]
[Index("FulltenKh", Name = "UQ__Khachhan__D6512A83F236E621", IsUnique = true)]
public partial class Khachhang
{
    [Key]
    [Column("MaKH")]
    [StringLength(10)]
    public string MaKh { get; set; } = null!;

    [Column("FulltenKH")]
    [StringLength(100)]
    public string FulltenKh { get; set; } = null!;

    [Column("SdtKH")]
    [StringLength(15)]
    public string SdtKh { get; set; } = null!;

    [Column("DiachiKH")]
    [StringLength(100)]
    public string DiachiKh { get; set; } = null!;

    [Column("MaTK")]
    public int? MaTk { get; set; }

    [InverseProperty("MaKhNavigation")]
    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    [ForeignKey("MaTk")]
    [InverseProperty("Khachhangs")]
    public virtual Account? MaTkNavigation { get; set; }
}
