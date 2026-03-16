using MyERP.Application.Modules.Inventory.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.CRM.DTOs
{
    public class CustomerDetailedHistoryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public decimal LifetimeValue { get; set; } // Updated by your PayOrderAsync
        public string Segment { get; set; } // e.g., "VIP", "New", "At Risk"
        public DateTime? LastPurchaseDate { get; set; }

        // --- Unified History (The Timeline) ---
        public List<TimelineItemDto> Timeline { get; set; } = new List<TimelineItemDto>();
    }
}
