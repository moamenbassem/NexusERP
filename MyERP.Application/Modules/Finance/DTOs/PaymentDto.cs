using MyERP.Domain.Entities.Finance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Application.Modules.Finance.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? ProcessedByUserId { get; set; }
    }
}
