using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.HR.DTOs
{
    public class TimeSheetDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalHours {  get; set; }
        public string? Note { get; set; }
        public TimesheetStatus Status { get; set; } = TimesheetStatus.Draft;
        public int? ApprovedById { get; set; }
    }
}
