using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Purcahsing.DTOs
{
    public class PurchasingOrderProductDto
    {
        public int ProductId { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }

    }
}
