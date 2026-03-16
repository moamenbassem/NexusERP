using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.CRM.DTOs
{
    public class TimelineItemDto
    {
        public int id {  get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // "Order", "Payment", "Interaction", "Support"
        public string Description { get; set; }
    }
}
