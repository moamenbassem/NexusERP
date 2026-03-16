using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Domain.Entities.Inventory
{
    public class InventoryLog
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ChangeAmount { get; set; }

        public string? Note { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    }
}
