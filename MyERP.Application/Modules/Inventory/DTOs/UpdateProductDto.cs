using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }

        public decimal CostPrice { get; set; }

        public string? SKU { get; set; } = string.Empty;
        public int? CategoryId { get; set; } = null;

    }
}
