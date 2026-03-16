using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Modules.CRM.DTOs;
using MyERP.Application.Modules.CRM.Interfaces;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.Identity;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]

    public class CRMController : ControllerBase
    {
        private readonly ICRMService service;
        private readonly UserManager<AppUser> userManager;
        public CRMController(ICRMService _service, UserManager<AppUser> _userManager)
        {
            service = _service;
            userManager = _userManager;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LogCustomerIntearaction(CreateCustomerInteractionDto customerInteractionDto)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                await service.LogCustomerIntearactionAsync(customerInteractionDto, UserId);
                return Ok("Interaction has been Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{customerId}")]
        public async Task<IActionResult> GetCustomerDetailedHistory(int customerId)
        {
            try
            {
                var customerhistory = await service.GetCustomerDetailedHistoryAsync(customerId);
                return Ok(customerhistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCustomersHistory()
        {
            try
            {
                var customershistory = await service.GetAllCustomersHistoryAsync();
                return Ok(customershistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{customerId}")]
        public async Task<IActionResult> GetCustomerInteractions(int customerId)
        {
            try
            {
                var customerhistory = await service.GetCustomerInteractionsAsync(customerId);
                return Ok(customerhistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
