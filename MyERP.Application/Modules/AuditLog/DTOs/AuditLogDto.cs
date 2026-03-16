using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.AuditLog.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public int? EmployeeID { get; set; } // The Staff member ID
        public string Action { get; set; } = string.Empty; // e.g., "UpdateStatus"
        public string EntityName { get; set; } = string.Empty; // e.g., "Order"
        public int EntityId { get; set; } // The ID of the order/product
        public DateTime Timestamp { get; set; }
        public string? Details { get; set; } // e.g., "Changed status from Pending to Shipped"
    }
}
