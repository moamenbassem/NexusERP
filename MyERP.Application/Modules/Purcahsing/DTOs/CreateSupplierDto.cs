using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.Purcahsing.DTOs
{
    public class CreateSupplierDto
    {
        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone format.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }
    }
}
