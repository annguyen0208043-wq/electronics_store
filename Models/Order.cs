using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public string OrderId { get; set; } = Guid.NewGuid().ToString("N");

        [Required]
        [Column("UserId")]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}