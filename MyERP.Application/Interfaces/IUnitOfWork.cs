using MyERP.Domain.Entities.AuditLog;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using MyERP.Domain.Entities.Inventory;
using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepo { get; }
        IRepository<InventoryLog> InventoryLogRepo { get; }
        IRepository<Category> CategoryRepo { get; }
        IRepository<Order> OrderRepo { get; }
        IRepository<OrderProduct> OrderProductRepo { get; }
        IRepository<Supplier> SupplierRepo { get; }
        IRepository<PurchasingOrder> PurchasingOrderRepo { get; }
        IRepository<PurchasingOrderProduct> PurchasingOrderProductRepo { get; }
        IRepository<AppUser> AppUserRepo { get; }
        IRepository<AuditLog> AuditLogRepo { get; }
        IRepository<Invoice> InvoiceRepo { get; }
        IRepository<Payment> PaymentRepo { get; }
        IRepository<Customer> CustomerRepo { get; }
        IRepository<Employee> EmployeeRepo { get; }
        IRepository<CustomerInteraction> CustomerInteractionRepo { get; }
        IRepository<TimeSheet> TimeSheetRepo { get; }
        IRepository<SalaryPayment> SalaryPaymentRepo { get; }

        public Task Commit();
    }
}
