using MyERP.Domain.Entities;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? PaidAt { get; set; } 
        public IEnumerable<OrderProductDto>? orderProducts { get; set; } = new List<OrderProductDto>();
        public decimal TotalCost { get; set; }


    }
}
