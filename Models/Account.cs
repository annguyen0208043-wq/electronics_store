using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Models;

[Index("UsernameTk", Name = "UQ__Accounts__15DC27FC668419E0", IsUnique = true)]
public partial class Account
{
    [Key]
    [Column("MaTK")]
    public int MaTk { get; set; }

    [Column("UsernameTK")]
    [StringLength(50)]
    public string UsernameTk { get; set; } = null!;

    [Column("PasswordTK")]
    [StringLength(255)]
    public string PasswordTk { get; set; } = null!;

    [Column("RoleTK")]
    public int RoleTk { get; set; }

    [StringLength(255)]
    public string Gmail { get; set; } = null!;

    [InverseProperty("MaTkNavigation")]
    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();

    [InverseProperty("MaTkNavigation")]
    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
