using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyERP.Domain.Entities.HR
{
    public class Employee : AppUser
    {
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public PayType PayType { get; set; }
        public decimal PayRate { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; } = EmployeeStatus.Probation;
        public virtual IEnumerable<SalaryPayment>? SalaryPayments { get; set; }
        [JsonIgnore]
        public virtual ICollection<MyERP.Domain.Entities.AuditLog.AuditLog>? Logs { get; set; } = new List<MyERP.Domain.Entities.AuditLog.AuditLog>();

    }

    public enum PayType
    {
        Monthly,
        Hourly,
        Intern
    }
    public enum EmployeeStatus
    {
        Active,
        OnLeave,
        Terminated,
        Probation
    }
}
