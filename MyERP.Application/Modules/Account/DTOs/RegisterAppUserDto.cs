using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Application.Modules.Account.DTOs
{
    public class RegisterAppUserDto
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
    }
}
