using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.Finance.DTOs;
using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Finance.Interfaces
{
    public interface IFinanceService
    {
        public Task GenerateInvoiceInternalAsync(Order order);
        public Task<SalaryPayment> GenerateSalaryPaymentAsync(int EmployeeId, decimal PayRate, int AdminId);
        public Task ProcessPaymentAsync(int invoiceId, int userId);
        public Task<IEnumerable<InvoiceDto>> CheckOverdueInvoicesAsync();
        public Task<InvoiceDto> GetOneInvoiceAsync(int id);
        public Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        public Task<IEnumerable<InvoiceDto>> GetAllUnpaidInvoicesAsync();
        public Task<PaymentDto> GetOnePaymentAsync(int InvoiceId);
        public Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        public Task<ProductSalesReportDto> GetProductSalesReportAsync(int id);
        public Task<CategorySalesReportDto> GetCategorySalesReportAsync(int id);
        public Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime startDate, DateTime endDate);


    }
}
