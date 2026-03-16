using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class ProductDto
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public int Quantity { get; set; }
        public string? SKU { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? CategoryId { get; set; }

    }
}
