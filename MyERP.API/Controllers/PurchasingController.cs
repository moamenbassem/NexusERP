using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Modules.Purcahsing.DTOs;
using MyERP.Application.Modules.Purcahsing.Interfaces;
using MyERP.Application.Modules.Purcahsing.Mappers;
using MyERP.Domain.Entities.Identity;
using System.ClientModel.Primitives;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class PurchasingController : ControllerBase
    {
        private readonly IPurchasingService Service;
        private readonly UserManager<AppUser> userManager;
        public PurchasingController(IPurchasingService _service,UserManager<AppUser> _userManager)
        {
            Service = _service;
            userManager = _userManager;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetOneSupplier(int id)
        {
            try
            {
                var supplier = await Service.GetOneSupplierAsync(id);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllSupplier()
        {
            try
            {
                var suppliers = await Service.GetAllSupplierAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPurchasingOrders()
        {
            try
            {
                var orders = await Service.GetAllPurchasingOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NewSupplier(CreateSupplierDto dto)
        {
            var supplier = await Service.NewSupplierAsync(dto);
            return Ok(supplier);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateSupplier(CreateSupplierDto dto, int id)
        {
            try
            {
                var supplier = await Service.UpdateSupplierAsync(dto, id);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateOrder(CreatePurchasingOrderDto dto, int id)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var order = await Service.UpdateOrderAsync(dto, id,UserID);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> UpdateSupplierPatch(JsonPatchDocument<CreateSupplierDto> PATCH, int id)
        {
            try
            {
                var supplier = await Service.UpdateSupplierPatchAsync(PATCH, id);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await Service.DeleteSupplierAsync(id);
                return Ok(new { Message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PlaceAnOrder(CreatePurchasingOrderDto CreateDto)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var order = await Service.PlaceAnOrderAsync(CreateDto,UserID);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpPost("[action]/{OrderId}")]
        public async Task<IActionResult> ReceiveOrder(int OrderId)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var order = await Service.ReceiveOrderAsync(OrderId,UserID);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpPost("[action]/{OrderId}")]
        public async Task<IActionResult> CancelOrder(int OrderId)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var order = await Service.CancelOrderAsync(OrderId,UserID);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


    }
}
