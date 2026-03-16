using MyERP.Application.Modules.Finance.DTOs;
using MyERP.Application.Modules.HR.DTOs;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.HR.Interfaces
{
    public interface IHRService
    {
        public Task<TimeSheetDto> SubmitTimeSheet(CreateTimeSheetDto dto, int EmployeeId);
        public Task<IEnumerable<TimeSheetDto>> GetOneEmployeeSubmittedTimeSheetsAsync(int EmployeeId);
        public Task<IEnumerable<TimeSheetDto>> GetAllEmployeesSubmittedTimeSheetsAsync();
        public Task<IEnumerable<TimeSheetDto>> GetAllEmployeesApprovedTimeSheetsAsync();
        public Task<IEnumerable<TimeSheetDto>> GetOneEmployeeApprovedTimeSheetsAsync(int EmployeeId);
        public Task<IEnumerable<TimeSheetDto>> GetAllEmployeesRejectedTimeSheetsAsync();
        public Task<IEnumerable<TimeSheetDto>> GetOneEmployeeRejectedTimeSheetsAsync(int EmployeeId);
        public Task<IEnumerable<TimeSheetDto>> GetAllEmployeesProcessedTimeSheetsAsync();
        public Task<IEnumerable<TimeSheetDto>> GetOneEmployeeProcessedTimeSheetsAsync(int EmployeeId);
        public Task<IEnumerable<TimeSheetDto>> GetOneEmployeeAllMonthTimeSheetsAsync(int EmployeeId,int Month);
        public Task<IEnumerable<TimeSheetDto>> GetAllEmployeesAllMonthTimeSheetsAsync(int Month);

        public Task<IEnumerable<SalaryPaymentDto>> GetOneEmployeeSalaryPaymentsAsync(int EmployeeId);

        public Task<IEnumerable<SalaryPaymentDto>> GetMySalaryPaymentsAsync(int UserId);
        public Task<TimeSheetDto> ApproveTimeSheetAsync(int TimeSheetId, int AdminId);
        public Task<TimeSheetDto> RejectTimeSheetAsync(int TimeSheetId, int AdminId);
        public Task ProcessOneTimeSheetAsync(TimeSheet TimeSheet, int AdminId);
        public Task ProcessEmployeeSalaryAsync(int EmployeeId, int AdminId);

    }
}
