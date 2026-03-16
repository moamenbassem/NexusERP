using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Finance.DTOs
{
    public class SalaryPaymentDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public int? ProcessedByAdminId { get; set; }
    }
}
