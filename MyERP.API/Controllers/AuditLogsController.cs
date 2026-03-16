using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.AuditLog.Interfaces;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService service;
        public AuditLogsController(IAuditLogService _service)
        {
            service = _service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            // Find every action ever taken on a specific Order or Product
            try
            {
                var logs = await service.GetAllHistoryAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("entity-history/{entityName}/{entityId}")]
        public async Task<IActionResult> GetEntityHistory(string entityName, int entityId)
        {
            try
            {
                var logs = await service.GetEntityHistoryAsync(entityName,entityId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
