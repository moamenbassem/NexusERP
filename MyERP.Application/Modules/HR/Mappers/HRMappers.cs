using MyERP.Application.Modules.HR.DTOs;
using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyERP.Application.Modules.HR.Mappers
{
    public static class HRMappers
    {
        public static TimeSheet ToTimeSheet (this CreateTimeSheetDto dto, int UserId)
        {
            var TimeSheet = new TimeSheet
            {
                EmployeeId = UserId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Note = dto.Note,
                Status = TimesheetStatus.Submitted
            };
            return TimeSheet;
        }
        public static TimeSheetDto ToDto (this TimeSheet timeSheet)
        {
            var dto = new TimeSheetDto
            {
                Id = timeSheet.Id,
                EmployeeId = timeSheet.EmployeeId,
                Date = timeSheet.Date,
                StartTime = timeSheet.StartTime,
                EndTime = timeSheet.EndTime,
                TotalHours = timeSheet.TotalHours,
                Note = timeSheet.Note,
                Status = timeSheet.Status,
                ApprovedById = timeSheet.ProcessedById
            };
            return dto;
        }

    }
}
