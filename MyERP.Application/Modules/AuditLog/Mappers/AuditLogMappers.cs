using MyERP.Application.Modules.AuditLog.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.AuditLog.Mappers
{
    public static class AuditLogMappers
    {
        public static AuditLogDto ToDto (this MyERP.Domain.Entities.AuditLog.AuditLog Log)
        {
            return new AuditLogDto
                  {
                    Id = Log.Id,
                    EmployeeID = Log.EmployeeId,
                    Timestamp = Log.Timestamp,
                    Action = Log.Action,
                    Details = Log.Details,
                    EntityId = Log.EntityId,
                    EntityName = Log.EntityName

                };
        }
    }
}
