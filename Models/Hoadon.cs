using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Hoadon")]
public partial class Hoadon
{
    [Key]
    [Column("MaHD")]
    [StringLength(10)]
    public string MaHd { get; set; } = null!;

    [Column("NgaylapHD")]
    public DateOnly NgaylapHd { get; set; }

    [Column("MaKH")]
    [StringLength(10)]
    public string? MaKh { get; set; }

    [Column("MaNV")]
    [StringLength(10)]
    public string? MaNv { get; set; }

    [Column("TongtienHD", TypeName = "decimal(18, 2)")]
    public decimal? TongtienHd { get; set; }

    [StringLength(200)]
    public string? Ghichu { get; set; }

    [Column("Trangthai")]
    [StringLength(50)]
    public string? Trangthai { get; set; } 

    [InverseProperty("MaHdNavigation")]
    public virtual ICollection<ChitietHd> ChitietHds { get; set; } = new List<ChitietHd>();

    [ForeignKey("MaKh")]
    [InverseProperty("Hoadons")]
    public virtual Khachhang? MaKhNavigation { get; set; }

    [ForeignKey("MaNv")]
    [InverseProperty("Hoadons")]
    public virtual Nhanvien? MaNvNavigation { get; set; }
}
