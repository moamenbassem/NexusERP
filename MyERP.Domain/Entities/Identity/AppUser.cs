using Microsoft.AspNetCore.Identity;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
namespace MyERP.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }

        public Gender? Gender { get; set; }

    }
}
