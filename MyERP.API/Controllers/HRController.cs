using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Modules.HR.DTOs;
using MyERP.Application.Modules.HR.Interfaces;
using MyERP.Domain.Entities.Identity;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        private readonly IHRService service;
        private readonly UserManager<AppUser> usermanager;
        public HRController(IHRService _service, UserManager<AppUser> _usermanager)
        {
            service = _service;
            usermanager = _usermanager;
        }

        [HttpPost("[action]")]
        [Authorize(Roles ="Staff")]
        public async Task<IActionResult> SubmitTimeSheet(CreateTimeSheetDto dto) 
        {
            var UserIdString = usermanager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                var TimeSheet = await service.SubmitTimeSheet(dto, UserId);
                return Ok(TimeSheet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{EmployeeId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetOneEmployeeSubmittedTimeSheets(int EmployeeId) 
        {
            try
            {
                var TimeSheets = await service.GetOneEmployeeSubmittedTimeSheetsAsync(EmployeeId);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{EmployeeId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetOneEmployeeApprovedTimeSheets(int EmployeeId) 
        {
            try
            {
                var TimeSheets = await service.GetOneEmployeeApprovedTimeSheetsAsync(EmployeeId);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{EmployeeId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetOneEmployeeRejectedTimeSheets(int EmployeeId) 
        {
            try
            {
                var TimeSheets = await service.GetOneEmployeeRejectedTimeSheetsAsync(EmployeeId);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{EmployeeId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetOneEmployeeProcessedTimeSheets(int EmployeeId) 
        {
            try
            {
                var TimeSheets = await service.GetOneEmployeeProcessedTimeSheetsAsync(EmployeeId);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{EmployeeId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetOneEmployeeAllMonthTimeSheets(int EmployeeId, int? month) 
        {

            var Month = month ?? DateTime.Now.Month;
            try
            {
                var TimeSheets = await service.GetOneEmployeeAllMonthTimeSheetsAsync(EmployeeId, Month);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        [Authorize(Roles ="Staff")]
        public async Task<IActionResult> GetMyTimeSheets(int? month) 
        {
            var UserIdString = usermanager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            var Month = month ?? DateTime.Now.Month;
            try
            {
                var TimeSheets = await service.GetOneEmployeeAllMonthTimeSheetsAsync(UserId, Month);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployeesSubmittedTimeSheets() 
        {
            try
            {
                var TimeSheets = await service.GetAllEmployeesSubmittedTimeSheetsAsync();
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployeesApprovedTimeSheets() 
        {
            try
            {
                var TimeSheets = await service.GetAllEmployeesApprovedTimeSheetsAsync();
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployeesRejectedTimeSheets() 
        {
            try
            {
                var TimeSheets = await service.GetAllEmployeesRejectedTimeSheetsAsync();
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployeesProcessedTimeSheetsAsync() 
        {
            try
            {
                var TimeSheets = await service.GetAllEmployeesProcessedTimeSheetsAsync();
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployeesAllMonthTimeSheetsAsync(int? month) 
        {
            var Month = month ?? DateTime.Now.Month;
            try
            {
                var TimeSheets = await service.GetAllEmployeesAllMonthTimeSheetsAsync(Month);
                return Ok(TimeSheets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{EmployeeId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetOneEmployeeSalaryPayments(int EmployeeId) 
        {
            try
            {
                var salaryPayments = await service.GetOneEmployeeSalaryPaymentsAsync(EmployeeId);
                return Ok(salaryPayments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetMySalaryPaymentsAsync()
        {
            var UserIdString = usermanager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                var salaryPayments = await service.GetOneEmployeeSalaryPaymentsAsync(UserId);
                return Ok(salaryPayments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ApproveTimeSheet(int id) 
        {
            var UserIdString = usermanager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                var TimeSheet = await service.ApproveTimeSheetAsync(id,UserId);
                return Ok(TimeSheet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> RejectTimeSheet(int id) 
        {
            var UserIdString = usermanager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                var TimeSheet = await service.RejectTimeSheetAsync(id,UserId);
                return Ok(TimeSheet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("[action]/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ProcessEmployeeSalary(int id) 
        {
            var UserIdString = usermanager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                await service.ProcessEmployeeSalaryAsync(id,UserId);
                return Ok("Salary Paid Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
