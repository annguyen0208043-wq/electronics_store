using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    [Table("Nhanvien")]
    public partial class Nhanvien
    {
        [Key]
        [Column("MaNV")]
        [StringLength(10)]
        public string MaNv { get; set; } = null!;

        [StringLength(100)]
        public string TenNv { get; set; } = null!;

        [Column("MaTK")]
        public int? MaTk { get; set; }

        [ForeignKey("MaTk")]
        [InverseProperty("Nhanviens")]
        public virtual Account? MaTkNavigation { get; set; }

        [InverseProperty("MaNvNavigation")]
        public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
    }
}