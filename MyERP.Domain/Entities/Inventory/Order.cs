using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Domain.Entities.Inventory
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AppUser")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer {  get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; } // Add this!

        public OrderStatus Status { get; set; }= OrderStatus.Pending;
        public virtual IEnumerable<OrderProduct>? orderProduct { get; set; } = new List<OrderProduct>();

        public DateTime? DeliveredAt { get; set; } 

        public decimal TotalCost { get; set; }


        public virtual Invoice? Invoice { get; set; }


    }
}
