using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Modules.Finance.Interfaces;
using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Domain.Entities.Finance;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService service;
        public FinanceController(IFinanceService _service)
        {
            service = _service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CheckOverdueInvoices()
        {
            try
            {
                var invoices = await service.CheckOverdueInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetOneInvoice(int id)
        {
            try
            {
                var invoice = await service.GetOneInvoiceAsync(id);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await service.GetAllInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUnpaidInvoices()
        {
            try
            {
                var invoice = await service.GetAllUnpaidInvoicesAsync();
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{InvoiceId}")]
        public async Task<IActionResult> GetOnePayment(int InvoiceId)
        {
            try
            {
                var Payment = await service.GetOnePaymentAsync(InvoiceId);
                return Ok(Payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var Payments = await service.GetAllPaymentsAsync();
                return Ok(Payments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductSalesReport(int id)
        {
            try
            {
                var report = await service.GetProductSalesReportAsync(id);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategorySalesReport(int id)
        {
            try
            {
                var report = await service.GetCategorySalesReportAsync(id);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDashboardSummary(DateTime? start, DateTime? end)
        {
            var startDate = start ?? DateTime.UtcNow.AddDays(-30);
            var endDate = end ?? DateTime.UtcNow;
            try
            {
                var report = await service.GetDashboardSummaryAsync(startDate, endDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

