using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.HR.DTOs
{
    public class CreateTimeSheetDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
    }
}
