using MyERP.Application.Interfaces;
using MyERP.Domain.Entities.AuditLog;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using MyERP.Domain.Entities.Inventory;
using MyERP.Domain.Entities.Purchasing;
using MyERP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public UnitOfWork(AppDbContext _db)
        {
            db = _db;
            ProductRepo = new MainRepository<Product>(_db);
            InventoryLogRepo = new MainRepository<InventoryLog>(_db);
            CategoryRepo = new MainRepository<Category>(_db);
            OrderRepo = new MainRepository<Order>(_db);
            OrderProductRepo = new MainRepository<OrderProduct>(_db);
            SupplierRepo = new MainRepository<Supplier>(_db);
            PurchasingOrderRepo = new MainRepository<PurchasingOrder>(_db);
            PurchasingOrderProductRepo = new MainRepository<PurchasingOrderProduct>(_db);
            AppUserRepo = new MainRepository<AppUser>(_db);
            AuditLogRepo = new MainRepository<AuditLog>(_db);
            InvoiceRepo = new MainRepository<Invoice>(_db);
            PaymentRepo = new MainRepository<Payment>(_db);
            CustomerRepo = new MainRepository<Customer>(_db);
            EmployeeRepo = new MainRepository<Employee>(_db);
            CustomerInteractionRepo = new MainRepository<CustomerInteraction>(_db);
            TimeSheetRepo = new MainRepository<TimeSheet>(_db);
            SalaryPaymentRepo = new MainRepository<SalaryPayment>(_db);
        }
        public readonly AppDbContext db;
        public IRepository<Product> ProductRepo {  get; private set; }

        public IRepository<InventoryLog> InventoryLogRepo {  get; private set; }
        public IRepository<Category> CategoryRepo { get; private set; }
        public IRepository<Order> OrderRepo { get; private set; }
        public IRepository<OrderProduct> OrderProductRepo { get; private set; }

        public IRepository<Supplier> SupplierRepo { get; private set; }

        public IRepository<PurchasingOrder> PurchasingOrderRepo { get; private set; }

        public IRepository<PurchasingOrderProduct> PurchasingOrderProductRepo { get; private set; }

        public IRepository<AppUser> AppUserRepo { get; private set; }

        public IRepository<AuditLog> AuditLogRepo { get; private set; }

        public IRepository<Invoice> InvoiceRepo { get; private set; }

        public IRepository<Payment> PaymentRepo { get; private set; }

        public IRepository<Customer> CustomerRepo { get; private set; }

        public IRepository<Employee> EmployeeRepo { get; private set; }
        public IRepository<CustomerInteraction> CustomerInteractionRepo { get; private set; }

        public IRepository<TimeSheet> TimeSheetRepo { get; private set; }

        public IRepository<SalaryPayment> SalaryPaymentRepo { get; private set; }

        public async Task Commit()
        {
           await db.SaveChangesAsync();
        }

        public async void Dispose()
        {
           await db.DisposeAsync();
        }
    }
}
