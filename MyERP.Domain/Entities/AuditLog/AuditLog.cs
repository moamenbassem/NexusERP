using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyERP.Domain.Entities.AuditLog
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; } // The Staff member ID
        [JsonIgnore]
        public virtual Employee? Employee { get; set; }

        public string Action { get; set; } = string.Empty; // e.g., "UpdateStatus"
        public string EntityName { get; set; } = string.Empty; // e.g., "Order"
        public int EntityId { get; set; } // The ID of the order/product

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Details { get; set; } // e.g., "Changed status from Pending to Shipped"
    }
}
