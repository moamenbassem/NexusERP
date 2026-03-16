using MyERP.Application.Modules.CRM.DTOs;
using MyERP.Domain.Entities.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.CRM.Interfaces
{
    public interface ICRMService
    {
        public Task LogCustomerIntearactionAsync(CreateCustomerInteractionDto customerInteractionDto, int employeeId);
        public Task InternalLogCustomerIntearactionAsync(CreateCustomerInteractionDto customerInteractionDto, int employeeId);

        public Task AutoLogCustomerIntearactionAsync(CreateCustomerInteractionDto customerInteractionDto);
        public Task<CustomerDetailedHistoryDto> GetCustomerDetailedHistoryAsync(int customerId);
        public Task<IEnumerable<CustomerHistoryDto>> GetAllCustomersHistoryAsync();
        public Task<IEnumerable<CustomerInteractionDto>> GetCustomerInteractionsAsync(int customerId);

    }
}
