using MyERP.Application.Modules.Finance.DTOs;
using MyERP.Domain.Entities.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Finance.Mappers
{
    public static class FinanceMappers
    {
        public static InvoiceDto ToInvoiceDto(this Invoice invoice)
        {
            return new InvoiceDto
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                AmountDue = invoice.AmountDue,
                IssuedAt = invoice.IssuedAt,
                DueDate = invoice.DueDate,
                InvoiceNumber = invoice.InvoiceNumber,
                Status = invoice.Status
            };
        }
        public static PaymentDto ToPaymentDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                InvoiceId = payment.InvoiceId,
                ProcessedByUserId = payment.ProcessedByUserId
            };
        }
        public static SalaryPaymentDto ToSalaryPaymentDto(this SalaryPayment payment)
        {
            return new SalaryPaymentDto
            {
                Id = payment.Id,
                EmployeeId = payment.EmployeeId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                ProcessedByAdminId = payment.ProcessedByAdminId
            };
        }

    }
}
