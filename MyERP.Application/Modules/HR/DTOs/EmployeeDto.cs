using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.HR.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public PayType PayType { get; set; }
        public decimal PayRate { get; set; }
        public string? Roles { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; } = EmployeeStatus.Probation;
    }
}
