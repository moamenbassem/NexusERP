using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Application.Modules.Finance.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal AmountDue { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}
