using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Domain.Entities.Inventory
{
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Paid
    }
}
