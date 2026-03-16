using MyERP.Application.Modules.Inventory.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Finance.DTOs
{
    public class DashboardSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public int PendingOrdersCount { get; set; }
        public decimal UnpaidInvoicesAmount { get; set; }

        public List<TopProductDto> TopSellingProducts { get; set; }
    }
}
