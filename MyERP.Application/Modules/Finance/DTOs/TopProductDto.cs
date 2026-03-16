using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Finance.DTOs
{
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenueGenerated { get; set; }
    }
}
