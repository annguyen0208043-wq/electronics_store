using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Table("ChitietHD")]
public partial class ChitietHd
{
    [Key]
    [Column("MaCTHD")]
    [StringLength(10)]
    public string MaCthd { get; set; } = null!;

    [Column("MaHD")]
    [StringLength(10)]
    public string MaHd { get; set; } = null!;

    [Column("MaHH")]
    [StringLength(10)]
    public string MaHh { get; set; } = null!;

    public int Soluong { get; set; }

    public int Dongia { get; set; }

    [ForeignKey("MaHd")]
    [InverseProperty("ChitietHds")]
    public virtual Hoadon MaHdNavigation { get; set; } = null!;

    [ForeignKey("MaHh")]
    [InverseProperty("ChitietHds")]
    public virtual Hanghoa MaHhNavigation { get; set; } = null!;
}
