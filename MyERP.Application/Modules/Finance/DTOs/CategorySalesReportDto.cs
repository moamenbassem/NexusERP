using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Finance.DTOs
{
    public class CategorySalesReportDto
    {
        public int CategoryId { get; set; }
        public int TotalSoldUnits { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Profit { get; set; }
    }
}
