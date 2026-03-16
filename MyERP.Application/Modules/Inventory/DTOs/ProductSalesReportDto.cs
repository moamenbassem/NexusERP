using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class ProductSalesReportDto
    {
        public int ProductId { get; set; }
        public int TotalSoldUnits { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Profit { get; set; }

    }
}
