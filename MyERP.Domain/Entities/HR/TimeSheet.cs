using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyERP.Domain.Entities.HR
{
    public class TimeSheet
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalHours => (decimal)(EndTime - StartTime).TotalHours - (1m);

        public string? Note { get; set; }
        public TimesheetStatus Status { get; set; } = TimesheetStatus.Draft;
        public int? ProcessedById { get; set; }
    }

    public enum TimesheetStatus
    {
        Draft,
        Submitted,
        Approved,
        Processed,
        Rejected
    }
}
