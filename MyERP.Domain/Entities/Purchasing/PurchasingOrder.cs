using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Domain.Entities.Purchasing
{
    public class PurchasingOrder
    {
        [Key]
        public int Id { get; set; }

        public POStatus Status { get; set; } = POStatus.Draft;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RecievedAt { get; set; } = null;

        [ForeignKey("supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier? supplier { get; set; }

        public virtual ICollection<PurchasingOrderProduct>? purchasingOrderProduct { get; set; } = new List<PurchasingOrderProduct>();

        public decimal TotalCost { get; set; }
    }
}
