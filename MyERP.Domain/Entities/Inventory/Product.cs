using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Domain.Entities.Inventory
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal CurrentPrice { get; set; }
        [Required]
        public int Quantity { get; set; }

        public decimal CurrentCostPrice { get; set; }

        public string? SKU { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("category")]
        public int? CategoryId { get; set; } = null;
        public virtual Category? category { get; set; }

        public virtual IEnumerable<OrderProduct>? orderProduct { get; set; }
        public virtual IEnumerable<PurchasingOrderProduct>? purchasingOrderProduct { get; set; }

    }
}
