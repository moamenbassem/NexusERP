using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.AuditLog.DTOs;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.AuditLog.Mappers;
using MyERP.Domain.Entities.AuditLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Infrastructure.Modules.AuditLog
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork myunit;
        public AuditLogService(IUnitOfWork _my)
        {
            myunit = _my;
        }
        public async Task LogAsync(int? userId, string action, string entity, int entityId, string? details = null)
        {
            var log = new MyERP.Domain.Entities.AuditLog.AuditLog
            {
                EmployeeId = userId,
                Action = action,
                EntityName = entity,
                EntityId = entityId,
                Details = details,
                Timestamp = DateTime.Now
            };

            await myunit.AuditLogRepo.AddAsync(log);
        }
        public async Task<IEnumerable<AuditLogDto>> GetAllHistoryAsync()
        {
            var logs = await myunit.AuditLogRepo.GetAllAsync();
            if (logs == null) throw new Exception("There Are No logs");
            return logs.Select(x => x.ToDto());
        }
        public async Task<IEnumerable<AuditLogDto>> GetEntityHistoryAsync(string entityName, int entityId)
        {
            // Find every action ever taken on a specific Order or Product
            var logs = await myunit.AuditLogRepo.FindAsync(x => x.EntityName == entityName && x.EntityId == entityId);
            if (logs == null) throw new Exception("There Are No logs for that Entity");
            return logs.Select(x => x.ToDto());
        }
    }
}
