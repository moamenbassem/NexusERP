using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Modules.Account.DTOs;
using MyERP.Application.Modules.Account.Interfaces;
using MyERP.Application.Modules.HR.DTOs;
using MyERP.Domain.Entities.Identity;
using MyERP.Infrastructure.Modules.Account.Services;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;
        private readonly UserManager<AppUser> userManager;
        public AccountController(IAccountService _Service, UserManager<AppUser> _userManager)
        {
            service = _Service;
            userManager = _userManager;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterNewUser(RegisterAppUserDto dto)
        {
            try
            {
                var user = await service.RegisterNewUserAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> RegisterNewStaff(RegisterEmployeeDto dto)
        {
            var userIdString = userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();
            int userId = int.Parse(userIdString);
            try
            {
                var staff = await service.RegisterNewStaffAsync(dto, userId);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("[action]/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AssignNewAdminAsync(int id)
        {
            var userIdString = userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();
            int userId = int.Parse(userIdString);
            try
            {
                await service.AssignNewAdminAsync(id, userId);
                return Ok($"Employee of Id: {id} is Assigned New Admin");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await service.LoginAsync(dto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var Users = await service.GetAllUsersAsync();
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var Users = await service.GetAllCustomersAsync();
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var staff = await service.GetAllStaffAsync();
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
