using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Domain.Entities.Purchasing
{
    public class PurchasingOrderProduct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal CostPrice { get; set; }  
        public int PurchasingOrderId { get; set; }
        public virtual PurchasingOrder? purchasingOrder { get; set; }

        public int ProductId { get; set; }
        public virtual Product? product { get; set; }

    }
}
