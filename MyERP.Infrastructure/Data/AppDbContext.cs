using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using MyERP.Domain.Entities.Inventory;
using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> products { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchasingOrder> PurchasingOrders { get; set; }
        public DbSet<PurchasingOrderProduct> PurchasingOrderProducts { get; set; }
        public DbSet<MyERP.Domain.Entities.AuditLog.AuditLog> AuditLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CustomerInteraction> CustomerInteraction { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<SalaryPayment> SalaryPayments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<InventoryLog>().Property(x => x.TransactionDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<PurchasingOrder>().Property(x=> x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<PurchasingOrder>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<AppUser>().Property(p => p.Gender).HasConversion<string>();
            modelBuilder.Entity<Invoice>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<CustomerInteraction>().Property(p => p.Type).HasConversion<string>();
            modelBuilder.Entity<Employee>().Property(p => p.EmployeeStatus).HasConversion<string>();
            modelBuilder.Entity<Employee>().Property(p => p.PayType).HasConversion<string>();
            modelBuilder.Entity<TimeSheet>().Property(p => p.Status).HasConversion<string>();
            
            modelBuilder.Entity<AppUser>()
            .HasDiscriminator<string>("UserType")
            .HasValue<AppUser>("Internal")
            .HasValue<Customer>("Customer")
            .HasValue<Employee>("Employee");
           
            modelBuilder.Entity<IdentityRole<int>>().HasData
            (new IdentityRole<int>
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "1ad75b7b-83c3-4d6d-8884-29777f978001"


            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "2bd86c8c-94d4-5e7e-9995-30888g089112"

            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = "Staff",
                NormalizedName = "STAFF",
                ConcurrencyStamp = "3cd56r5r-83c3-3u5u-2226-03222k027669"

            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
