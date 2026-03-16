using MyERP.Domain.Entities.Identity;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Domain.Entities.CRM
{
    public class Customer : AppUser
    {
        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
        public decimal LifetimeValue { get; set; }
        public virtual ICollection<CustomerInteraction> Interactions { get; set; } = new List<CustomerInteraction>();
        //public CustomerStatus Status { get; set; }
    }
}
