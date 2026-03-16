using MyERP.Application.Interfaces;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.Finance.DTOs;
using MyERP.Application.Modules.Finance.Interfaces;
using MyERP.Application.Modules.Finance.Mappers;
using MyERP.Application.Modules.HR.DTOs;
using MyERP.Application.Modules.HR.Interfaces;
using MyERP.Application.Modules.HR.Mappers;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Infrastructure.Modules.HR
{
    public class HRService : IHRService
    {
        private readonly IUnitOfWork myunit;
        private readonly IAuditLogService auditLogService;
        private readonly IFinanceService financeService;
        public HRService(IUnitOfWork _myunit, IAuditLogService _auditLogService, IFinanceService _financeService)
        {
            myunit = _myunit;
            auditLogService = _auditLogService;
            financeService = _financeService;
        }
        public async Task<TimeSheetDto> SubmitTimeSheet(CreateTimeSheetDto dto, int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            if (Employee.PayType != PayType.Hourly)
                throw new Exception("Time Sheets are Specified for Hourly paid Employees only");
            var timeSheet = dto.ToTimeSheet(EmployeeId);
            timeSheet.Status = TimesheetStatus.Submitted;

            await myunit.TimeSheetRepo.AddAsync(timeSheet);
            await myunit.Commit();
            await auditLogService.LogAsync(EmployeeId, "Submitting TimeSheet", "TimeSheet",timeSheet.Id);
            await myunit.Commit();
            return timeSheet.ToDto();
        }
        public async Task<IEnumerable<TimeSheetDto>> GetOneEmployeeSubmittedTimeSheetsAsync(int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            if (Employee.PayType != PayType.Hourly)
                throw new Exception("Time Sheets are Specified for Hourly paid Employees only");

            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.EmployeeId == EmployeeId && x.Status == TimesheetStatus.Submitted);
            return TimeSheets.Select(x => x.ToDto());

        }
        public async Task<IEnumerable<TimeSheetDto>> GetOneEmployeeApprovedTimeSheetsAsync(int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            if (Employee.PayType != PayType.Hourly)
                throw new Exception("Time Sheets are Specified for Hourly paid Employees only");

            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.EmployeeId == EmployeeId && x.Status == TimesheetStatus.Approved);
            return TimeSheets.Select(x => x.ToDto());

        }
        public async Task<IEnumerable<TimeSheetDto>> GetOneEmployeeRejectedTimeSheetsAsync(int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            if (Employee.PayType != PayType.Hourly)
                throw new Exception("Time Sheets are Specified for Hourly paid Employees only");

            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.EmployeeId == EmployeeId && x.Status == TimesheetStatus.Rejected);
            return TimeSheets.Select(x => x.ToDto());

        }
        public async Task<IEnumerable<TimeSheetDto>> GetOneEmployeeProcessedTimeSheetsAsync(int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            if (Employee.PayType != PayType.Hourly)
                throw new Exception("Time Sheets are Specified for Hourly paid Employees only");

            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.EmployeeId == EmployeeId && x.Status == TimesheetStatus.Processed);
            return TimeSheets.Select(x => x.ToDto());

        }
        public async Task<IEnumerable<TimeSheetDto>> GetOneEmployeeAllMonthTimeSheetsAsync(int EmployeeId, int Month)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            if (Employee.PayType != PayType.Hourly)
                throw new Exception("Time Sheets are Specified for Hourly paid Employees only");

            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.EmployeeId == EmployeeId && x.Date.Month == Month);
            if (TimeSheets == null) throw new Exception($"There're No Time Sheets for that Month: {Month}");

            return TimeSheets.Select(x => x.ToDto());
        }

        public async Task<IEnumerable<TimeSheetDto>> GetAllEmployeesSubmittedTimeSheetsAsync()
        {
            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.Status == TimesheetStatus.Submitted);
            if (TimeSheets == null) throw new Exception("There're no Submitted Timesheets");
            return TimeSheets.Select(x => x.ToDto());
        }
        public async Task<IEnumerable<TimeSheetDto>> GetAllEmployeesApprovedTimeSheetsAsync()
        {
            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.Status == TimesheetStatus.Approved);
            if (TimeSheets == null) throw new Exception("There're no Approved Timesheets");
            return TimeSheets.Select(x => x.ToDto());
        }
        public async Task<IEnumerable<TimeSheetDto>> GetAllEmployeesRejectedTimeSheetsAsync()
        {
            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.Status == TimesheetStatus.Rejected);
            if (TimeSheets == null) throw new Exception("There're no Rejected Timesheets");
            return TimeSheets.Select(x => x.ToDto());
        }
        public async Task<IEnumerable<TimeSheetDto>> GetAllEmployeesProcessedTimeSheetsAsync()
        {
            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.Status == TimesheetStatus.Processed);
            if (TimeSheets == null) throw new Exception("There're no Processed Timesheets");
            return TimeSheets.Select(x => x.ToDto());
        }
        public async Task<IEnumerable<TimeSheetDto>> GetAllEmployeesAllMonthTimeSheetsAsync(int Month)
        {
            var TimeSheets = await myunit.TimeSheetRepo.FindAsync(x => x.Date.Month == Month);
            if (TimeSheets == null) throw new Exception($"There're No Time Sheets for that Month: {Month}");
            return TimeSheets.Select(x => x.ToDto());
        }

        public async Task<IEnumerable<SalaryPaymentDto>> GetOneEmployeeSalaryPaymentsAsync(int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");

            var SalaryPayments = await myunit.SalaryPaymentRepo.FindAsync(x=>x.EmployeeId == EmployeeId);
            if (SalaryPayments == null || !SalaryPayments.Any()) throw new Exception($"There're no Salary Payments for employee #{Employee.Id}");

            return SalaryPayments.Select(x=> x.ToSalaryPaymentDto());
        }
        public async Task<IEnumerable<SalaryPaymentDto>> GetMySalaryPaymentsAsync(int EmployeeId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");

            var SalaryPayments = await myunit.SalaryPaymentRepo.FindAsync(x=>x.EmployeeId == EmployeeId);
            if (SalaryPayments == null || !SalaryPayments.Any()) throw new Exception($"There're no Salary Payments for employee #{Employee.Id}");

            return SalaryPayments.Select(x=> x.ToSalaryPaymentDto());
        }
        public async Task<TimeSheetDto> ApproveTimeSheetAsync(int TimeSheetId, int AdminId)
        {
            var timeSheet = await myunit.TimeSheetRepo.GetByIdAsync(TimeSheetId);
            if (timeSheet == null) throw new Exception($"TimeSheet of this Id: {TimeSheetId} cannot be found");
            if (timeSheet.Status == TimesheetStatus.Approved || timeSheet.Status == TimesheetStatus.Processed)
                throw new Exception($"Already Approved by Admin of Id: {timeSheet.ProcessedById}");
            if (timeSheet.Status == TimesheetStatus.Rejected)
                throw new Exception($"Rejected by Admin of Id: {timeSheet.ProcessedById}");

            timeSheet.Status = TimesheetStatus.Approved;
            timeSheet.ProcessedById = AdminId;
            myunit.TimeSheetRepo.Update(timeSheet);
            await auditLogService.LogAsync(AdminId, $"Approving Employee: {timeSheet.EmployeeId} TimeSheet", "TimeSheet", timeSheet.Id);
            await myunit.Commit();
            return timeSheet.ToDto();
        }
        public async Task<TimeSheetDto> RejectTimeSheetAsync(int TimeSheetId, int AdminId)
        {
            var timeSheet = await myunit.TimeSheetRepo.GetByIdAsync(TimeSheetId);
            if (timeSheet == null) throw new Exception($"TimeSheet of this Id: {TimeSheetId} cannot be found");
            if (timeSheet.Status == TimesheetStatus.Approved || timeSheet.Status == TimesheetStatus.Processed)
                throw new Exception($"Already Approved by Admin of Id: {timeSheet.ProcessedById}");
            if (timeSheet.Status == TimesheetStatus.Rejected)
                throw new Exception($"Already Rejected by Admin of Id: {timeSheet.ProcessedById}");
           
            timeSheet.Status = TimesheetStatus.Rejected;
            timeSheet.ProcessedById = AdminId;
            myunit.TimeSheetRepo.Update(timeSheet);
            await auditLogService.LogAsync(AdminId, $"Rejecting Employee: {timeSheet.EmployeeId} TimeSheet", "TimeSheet", timeSheet.Id);
            await myunit.Commit();
            return timeSheet.ToDto();
        }
        public async Task ProcessOneTimeSheetAsync(TimeSheet TimeSheet, int AdminId)
        {
            TimeSheet.Status = TimesheetStatus.Processed;
            TimeSheet.ProcessedById = AdminId;
            myunit.TimeSheetRepo.Update(TimeSheet);
        }

        public async Task ProcessEmployeeSalaryAsync(int EmployeeId, int AdminId)
        {
            var Employee = await myunit.EmployeeRepo.GetByIdAsync(EmployeeId);
            if (Employee == null) throw new Exception($"Employee of Id: {EmployeeId} Cannot be found");
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            if (Employee.PayType == PayType.Monthly)
            {
                bool AlreadyPaid = Employee.SalaryPayments?.Any(x => x.PaymentDate.Year == currentYear && x.PaymentDate.Month == currentMonth) ?? false; 
                if(AlreadyPaid)
                    throw new Exception("This month Salary Is already paid");

                var SP = await financeService.GenerateSalaryPaymentAsync(Employee.Id, Employee.PayRate, AdminId);
                await myunit.Commit();
                await auditLogService.LogAsync(AdminId, $"Monthly Salary Payment for Employee: #{Employee.Id}", "SalaryPayment", SP.Id);

            }

            if (Employee.PayType == PayType.Hourly) 
            {
                var timeSheets = await myunit.TimeSheetRepo.FindAsync(x=> x.EmployeeId == Employee.Id && x.Status==TimesheetStatus.Approved);

                decimal TotalAmount = 0;
                foreach (var timeSheet in timeSheets)
                {
                    await ProcessOneTimeSheetAsync(timeSheet, AdminId);
                    TotalAmount += timeSheet.TotalHours * Employee.PayRate;
                }
                var SP = await financeService.GenerateSalaryPaymentAsync(EmployeeId, TotalAmount, AdminId);
                await myunit.Commit();
                await auditLogService.LogAsync(AdminId, $"Monthly Salary Payment for Hourly-Paid Employee: #{Employee.Id}", "SalaryPayment", SP.Id);

            }
            if (Employee.EmployeeStatus == EmployeeStatus.Probation && Employee.SalaryPayments?.Count() >= 3) 
            {
                Employee.EmployeeStatus = EmployeeStatus.Active;
                await auditLogService.LogAsync(AdminId, $"Employee #{Employee.Id} promoted to Active status.", "Employee", Employee.Id);
            }

            await myunit.Commit();


        }





    }
}
