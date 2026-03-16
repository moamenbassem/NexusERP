using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.HR.DTOs
{
    public class RegisterEmployeeDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public PayType PayType { get; set; }
        public decimal PayRate { get; set; }
    }
}
