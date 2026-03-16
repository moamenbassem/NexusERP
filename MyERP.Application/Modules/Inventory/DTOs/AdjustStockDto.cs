using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class AdjustStockDto
    {
        public int ProductId { get; set; }
        public int ChangeAmount { get; set; }

        public string? Reason { get; set; } = string.Empty;
    }
}
