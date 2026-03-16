using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyERP.Domain.Entities.Purchasing
{
    public enum POStatus
    {
        Draft,      // Still editing
        Ordered,    // Sent to supplier
        Received,   // Goods arrived, stock updated
        Cancelled
    }
}
