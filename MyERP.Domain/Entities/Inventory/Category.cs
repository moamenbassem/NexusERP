using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Domain.Entities.Inventory
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;

        public virtual IEnumerable<Product>? Products { get; set; }

    }
}
