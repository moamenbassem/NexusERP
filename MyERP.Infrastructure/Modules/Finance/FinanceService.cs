using MyERP.Application.Interfaces;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.Finance.DTOs;
using MyERP.Application.Modules.Finance.Interfaces;
using MyERP.Application.Modules.Finance.Mappers;
using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Inventory;
using MyERP.Infrastructure.Modules.AuditLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Infrastructure.Modules.Finance
{
    public class FinanceService : IFinanceService
    {
        private readonly IUnitOfWork myunit;
        private readonly IAuditLogService auditLogService;

        public FinanceService(IUnitOfWork _myunit, IAuditLogService _auditLogService)
        {
            myunit = _myunit;
            auditLogService = _auditLogService;
        }
        public async Task<IEnumerable<InvoiceDto>> CheckOverdueInvoicesAsync()
        {
            var unpaidInvoices = await myunit.InvoiceRepo.FindAsync(
                x => x.Status == InvoiceStatus.Unpaid
            );
            if (unpaidInvoices == null || !unpaidInvoices.Any()) throw new Exception("There are no OverdueInvoices");

            var overdueInvoices = unpaidInvoices.Where(x => x.DueDate < DateTime.UtcNow);
            if (overdueInvoices == null || !overdueInvoices.Any()) throw new Exception("There are no OverdueInvoices");

            foreach (var invoice in overdueInvoices)
            {
                invoice.Status = InvoiceStatus.Overdue;
                myunit.InvoiceRepo.Update(invoice);

                // Log it so you know exactly when the system flagged it
                await auditLogService.LogAsync(null, "Invoice Flagged Overdue", "Invoice", invoice.Id);
            }

            await myunit.Commit();
            return overdueInvoices.Select(x => x.ToInvoiceDto());
        }
        public async Task GenerateInvoiceInternalAsync(Order order)
        {
            var invoice = new Invoice()
            {
                OrderId = order.Id,
                InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmm}-{order.Id.ToString("D4")}",
                AmountDue = order.TotalCost,
                DueDate = DateTime.UtcNow.AddDays(30),
                IssuedAt = DateTime.UtcNow,
                Status = InvoiceStatus.Unpaid
            };

            await myunit.InvoiceRepo.AddAsync(invoice);
        }
        public async Task<SalaryPayment> GenerateSalaryPaymentAsync(int EmployeeId, decimal PayRate,int AdminId)
        {
            var SP = new SalaryPayment
            {
                EmployeeId = EmployeeId,
                Amount = PayRate,
                ProcessedByAdminId = AdminId
            };

            await myunit.SalaryPaymentRepo.AddAsync(SP);
            return SP;
        }


        public async Task ProcessPaymentAsync(int invoiceId, int userId)
        {
            var invoice = await myunit.InvoiceRepo.GetByIdAsync(invoiceId);

            if (invoice == null) throw new Exception("Invoice not found.");

            var payment = new Payment
            {
                InvoiceId = invoice.Id,
                Amount = invoice.AmountDue,
                ProcessedByUserId = userId,
                PaymentDate = DateTime.UtcNow
            };

            invoice.Status = InvoiceStatus.Paid;
            invoice.IsSettled = true;
            invoice.Payment = payment;

            if (invoice.Order != null)
            {
                invoice.Order.Status = OrderStatus.Paid;
            }

            await myunit.PaymentRepo.AddAsync(payment);
            myunit.InvoiceRepo.Update(invoice);



        }
        public async Task<InvoiceDto> GetOneInvoiceAsync(int id)
        {
            var invoice = await myunit.InvoiceRepo.GetByIdAsync(id);
            if (invoice == null) throw new Exception("There are no Invoices");
            return invoice.ToInvoiceDto();
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await myunit.InvoiceRepo.GetAllAsync();
            if (invoices == null || !invoices.Any()) throw new Exception("There are no Invoices");
            return invoices.Select(x => x.ToInvoiceDto());
        }
        public async Task<IEnumerable<InvoiceDto>> GetAllUnpaidInvoicesAsync()
        {
            var invoices = await myunit.InvoiceRepo.FindAsync(x => x.Status == InvoiceStatus.Unpaid);
            if (invoices == null || !invoices.Any()) throw new Exception("There are no Unpaid Invoices");
            return invoices.Select(x => x.ToInvoiceDto());
        }
        public async Task<PaymentDto> GetOnePaymentAsync(int InvoiceId)
        {
            var payment = await myunit.PaymentRepo.FindAsync(x => x.InvoiceId == InvoiceId);
            if (payment == null) throw new Exception($"There are no payments for Invoice of id: {InvoiceId}");
            return payment.First().ToPaymentDto();
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await myunit.PaymentRepo.GetAllAsync();
            if (payments == null || !payments.Any()) throw new Exception("There are no payments");
            return payments.Select(x => x.ToPaymentDto());
        }


        public async Task<ProductSalesReportDto> GetProductSalesReportAsync(int id)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(id);
            if (product == null)
                throw new Exception($"No Product of this Id: {id}");
            if (!product.orderProduct.Any())
                return new ProductSalesReportDto { ProductId = product.Id, TotalSoldUnits = 0, TotalRevenue = 0, Profit = 0 };
            var Report = new ProductSalesReportDto
            {
                ProductId = product.Id,
                TotalSoldUnits = 0,
                TotalRevenue = 0,
                Profit = 0
            };
            foreach (var op in product.orderProduct)
            {
                if (op.order.Status == OrderStatus.Paid) 
                {
                    Report.TotalSoldUnits += op.Qty;
                    Report.TotalRevenue += (op.Qty * op.Price);
                    Report.Profit += op.Qty * (op.Price - op.CostPrice);
                }

            }
            return Report;
        }

        public async Task<CategorySalesReportDto> GetCategorySalesReportAsync(int id)
        {
            var Products = await myunit.ProductRepo.FindAsync(x => x.CategoryId == id);
            if (Products == null || !Products.Any())
                throw new Exception($"No Products in this Category of Id: {id}");

            var catReport = new CategorySalesReportDto
            {
                CategoryId = id,
                TotalSoldUnits = 0,
                TotalRevenue = 0,
                Profit = 0
            };

            foreach (var product in Products)
            {
                if (!product.orderProduct.Any())
                {
                    continue;
                }

                foreach (var op in product.orderProduct)
                {
                    if (op.order.Status == OrderStatus.Paid)
                    {
                        catReport.TotalSoldUnits += op.Qty;
                        catReport.TotalRevenue += (op.Qty * op.Price);
                        catReport.Profit += op.Qty * (op.Price - op.CostPrice);
                    }

                }    
            }
            return catReport;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var summary = new DashboardSummaryDto()
            {
                TotalRevenue = 0,
                TotalProfit = 0,
                PendingOrdersCount = 0,
                UnpaidInvoicesAmount = 0
            };
            var paidOrders = await myunit.OrderRepo.FindAsync(x=> x.Status == OrderStatus.Paid && x.Invoice.Payment.PaymentDate >= startDate && x.Invoice.Payment.PaymentDate <= endDate);

            if (paidOrders != null && paidOrders.Any())
            {
                var allSoldItems = paidOrders.SelectMany(o => o.orderProduct);

                // Group by Product and Select the Top 5
                summary.TopSellingProducts = allSoldItems
                    .GroupBy(item => new { item.ProductId, item.product.Name })
                    .Select(group => new TopProductDto
                    {
                        ProductId = group.Key.ProductId,
                        ProductName = group.Key.Name,
                        TotalQuantitySold = group.Sum(x => x.Qty),
                        TotalRevenueGenerated = group.Sum(x => x.Qty * x.Price)
                    })
                    .OrderByDescending(x => x.TotalQuantitySold)
                    .Take(5) 
                    .ToList();

                
                    summary.TotalRevenue = allSoldItems.Sum(x => (x.Price * x.Qty));
                    summary.TotalProfit  = allSoldItems.Sum(x => x.Qty * (x.Price - x.CostPrice));
                
            }


            // if status is pending --> Invoice is not issued yet
            var unpaidOrders = await myunit.OrderRepo.FindAsync(x => x.Status == OrderStatus.Shipped || x.Status == OrderStatus.Delivered);
            if (unpaidOrders != null)
            {
                summary.PendingOrdersCount = unpaidOrders.Count();

                foreach (var order in unpaidOrders)
                {
                    if(order.Invoice == null) continue;
                    summary.UnpaidInvoicesAmount += order.Invoice.AmountDue;
                }
            }


            return summary;
        }
    }
}
