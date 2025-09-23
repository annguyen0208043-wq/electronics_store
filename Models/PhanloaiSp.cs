using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("PhanloaiSP")]
public partial class PhanloaiSp
{
    [Key]
    [Column("MaPLSP")]
    [StringLength(10)]
    public string MaPlsp { get; set; } = null!;

    [Column("TenPLSP")]
    [StringLength(100)]
    public string TenPlsp { get; set; } = null!;

    [Column("MoTaPLSP")]
    [StringLength(200)]
    public string? MoTaPlsp { get; set; }

    [InverseProperty("MaPlspNavigation")]
    public virtual ICollection<Banphim> Banphims { get; set; } = new List<Banphim>();

    [InverseProperty("MaPlspNavigation")]
    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();

    [InverseProperty("MaPlspNavigation")]
    public virtual ICollection<Loa> Loas { get; set; } = new List<Loa>();

    [InverseProperty("MaPlspNavigation")]
    public virtual ICollection<Manhinh> Manhinhs { get; set; } = new List<Manhinh>();

    [InverseProperty("MaPlspNavigation")]
    public virtual ICollection<Tainghe> Tainghes { get; set; } = new List<Tainghe>();
}
