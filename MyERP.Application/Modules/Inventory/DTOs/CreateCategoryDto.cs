using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = String.Empty;
    }
}
