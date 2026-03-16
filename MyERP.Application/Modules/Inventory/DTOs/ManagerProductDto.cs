using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class ManagerProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }
        public string? SKU { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? CategoryId { get; set; }
    }
}
