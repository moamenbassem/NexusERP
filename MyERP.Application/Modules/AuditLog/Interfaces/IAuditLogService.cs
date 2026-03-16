using MyERP.Application.Modules.AuditLog.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.AuditLog.Interfaces
{
    public interface IAuditLogService
    {
        public Task LogAsync(int? userId, string action, string entity, int entityId, string? details = null);
        public Task<IEnumerable<AuditLogDto>> GetAllHistoryAsync();
        public Task<IEnumerable<AuditLogDto>> GetEntityHistoryAsync(string entityName, int entityId);


    }
}
