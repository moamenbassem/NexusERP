using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Purcahsing.DTOs
{
    public class PurchasingOrderDto
    {
        public int Id { get; set; }
        public POStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SupplierId { get; set; }
        public IEnumerable<PurchasingOrderProductDto>? purchasingOrderProduct { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? RecievedAt { get; set; } = null;



    }
}
