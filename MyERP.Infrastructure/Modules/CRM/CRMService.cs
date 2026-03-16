using Castle.Core.Resource;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.Account.Mappers;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.CRM.DTOs;
using MyERP.Application.Modules.CRM.Interfaces;
using MyERP.Application.Modules.CRM.Mappers;
using MyERP.Domain.Entities.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Infrastructure.Modules.CRM
{
    public class CRMService : ICRMService
    {
        private readonly IUnitOfWork myunit;
        private readonly IAuditLogService auditLogService;
        public CRMService(IUnitOfWork _myunit,IAuditLogService _auditLogService)
        {
            myunit = _myunit;
            auditLogService = _auditLogService;
        }
        public async Task LogCustomerIntearactionAsync(CreateCustomerInteractionDto customerInteractionDto, int employeeId)
        {
            await InternalLogCustomerIntearactionAsync(customerInteractionDto, employeeId);
            await auditLogService.LogAsync(employeeId, "Customer Interaction", "Customer", customerInteractionDto.CustomerId);
            await myunit.Commit();
        }

        public async Task InternalLogCustomerIntearactionAsync(CreateCustomerInteractionDto customerInteractionDto, int employeeId) 
        {
            var interaction = customerInteractionDto.ToCustomerInteraction(employeeId);
            await myunit.CustomerInteractionRepo.AddAsync(interaction);
        }

        public async Task AutoLogCustomerIntearactionAsync(CreateCustomerInteractionDto customerInteractionDto)
        {
            var interaction = new CustomerInteraction
            {
                CustomerId = customerInteractionDto.CustomerId,
                Note = customerInteractionDto.Note,
                CreatedAt = DateTime.UtcNow,
                Type = customerInteractionDto.Type,
                EmployeeId = null
            };
            await myunit.CustomerInteractionRepo.AddAsync(interaction);
        }
        public async Task<CustomerDetailedHistoryDto> GetCustomerDetailedHistoryAsync(int customerId)
        {
            var customer = await myunit.CustomerRepo.GetByIdAsync(customerId);
            if (customer == null) throw new Exception($"Customer of Id:{customerId} Cannot be found");

            return customer.ToDetailedHistoryDto();
        }
        public async Task<IEnumerable<CustomerHistoryDto>> GetAllCustomersHistoryAsync()
        {
            var customers = await myunit.CustomerRepo.GetAllAsync();
            if (customers == null) throw new Exception($"Customers Cannot be found");

            return customers.Select(x=> x.ToHistoryDto());
        }

        public async Task<IEnumerable<CustomerInteractionDto>> GetCustomerInteractionsAsync(int customerId)
        {
            var customer = await myunit.CustomerRepo.GetByIdAsync(customerId);
            if (customer == null) throw new Exception($"Customer of Id:{customerId} Cannot be found");

            var interactions = await myunit.CustomerInteractionRepo.FindAsync(x=> x.CustomerId == customerId);
            if (interactions == null || !interactions.Any()) throw new Exception($"Customer of Id:{customerId} has no interactions");

            return interactions.Select(x => x.ToCustomerInteractionDto());
        }

    }
}
