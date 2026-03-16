using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int InitialQuantity { get; set; }
        [Required]
        public decimal CostPrice { get; set; }

        public string? SKU { get; set; } = string.Empty;

        public int? CategoryId { get; set; } = null;
    }
}
