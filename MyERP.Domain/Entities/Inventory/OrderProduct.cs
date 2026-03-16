using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Domain.Entities.Inventory
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int Qty { get; set; }

        [ForeignKey("order")]
        public int OrderId { get; set; }
        public virtual Order? order { get; set; } = null;

        [ForeignKey("product")]
        public int ProductId { get; set; }
        public virtual Product? product { get; set; } = null;

        

    }
}
