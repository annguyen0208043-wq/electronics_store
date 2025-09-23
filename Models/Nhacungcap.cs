using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("Nhacungcap")]
public partial class Nhacungcap
{
    [Key]
    [Column("MaNCC")]
    [StringLength(10)]
    public string MaNcc { get; set; } = null!;

    [Column("TenNCC")]
    [StringLength(100)]
    public string TenNcc { get; set; } = null!;

    [Column("EmailNCC")]
    [StringLength(100)]
    public string EmailNcc { get; set; } = null!;

    [Column("SdtNCC")]
    [StringLength(15)]
    public string SdtNcc { get; set; } = null!;

    [Column("DiachiNCC")]
    [StringLength(200)]
    public string? DiachiNcc { get; set; }

    [StringLength(100)]
    public string Nguoilienlac { get; set; } = null!;

    [InverseProperty("MaNccNavigation")]
    public virtual ICollection<Hanghoa> Hanghoas { get; set; } = new List<Hanghoa>();
}
