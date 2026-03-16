using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Domain.Entities.Purchasing
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Address { get; set; }

        public virtual IEnumerable<PurchasingOrder>? purchasingOrder { get; set; }

    }
}
