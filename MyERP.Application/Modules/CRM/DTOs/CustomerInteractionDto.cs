using MyERP.Domain.Entities.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.CRM.DTOs
{
    public class CustomerInteractionDto
    {
        public int CustomerId { get; set; }
        public int? EmployeeId { get; set; }

        public string Note { get; set; } // e.g., "Customer called to check order status"
        public InteractionType Type { get; set; }
    }
}
