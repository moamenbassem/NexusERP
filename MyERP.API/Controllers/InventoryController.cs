using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Application.Modules.Inventory.Interfaces;
using MyERP.Application.Modules.Inventory.Mappers;
using MyERP.Domain.Entities;
using MyERP.Domain.Entities.Identity;
using MyERP.Infrastructure.Data;

namespace MyERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService Service;
        private readonly UserManager<AppUser> userManager;
        public InventoryController(IInventoryService _service, UserManager<AppUser> _userManager)
        {
            Service = _service;
            userManager = _userManager;
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProducts()
        {
            var Products = await Service.GetAllProductsAsync();
            return Products == null ? NotFound("No Products found.") : Ok(Products);
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ManagerGetAllProductsAsync()
        {
            var Products = await Service.ManagerGetAllProductsAsync();
            return Products == null ? NotFound("No Products found.") : Ok(Products);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await Service.GetAllCategoriesAsync();
            return categories == null ? NotFound("No categories found.") : Ok(categories);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserOrderHistoryAsync()
        {
            var userIdString = userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();
            int userId = int.Parse(userIdString);
            try
            {
                var orders = await Service.GetUserOrderHistoryAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var Orders = await Service.GetAllOrdersAsync();
            return Orders == null ? NotFound("No Orders found.") : Ok(Orders);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllInventoryLogs()
        {
            var logs = await Service.GetAllInventoryLogsAsync();
            return logs == null ? NotFound("No logs found.") : Ok(logs);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetOneProduct(int id)
        {

            var product = await Service.GetOneProductAsync(id);
            return product == null ? NotFound($"No product found for this Id: {id}.") : Ok(product);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetOneCategory(int id)
        {
            var category = await Service.GetOneCategoryAsync(id);
            return category == null ? NotFound($"No category found for this Id: {id}.") : Ok(category);
        }

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOneOrder(int id)
        {
            var order = await Service.GetOneOrderAsync(id);
            return order == null ? NotFound($"No order found for this {id}.") : Ok(order);
        }

        [HttpGet("[action]/{Category_id}")]
        public async Task<IActionResult> GetCategoryProducts(int Category_id)
        {
            var category = await Service.GetOneCategoryAsync(Category_id);
            if (category == null) return NotFound($"No category found for this Id: {Category_id}.");

            var products = await Service.GetCategoryProductsAsync(Category_id);
            return products == null ? NotFound($"No Products found for this Category {Category_id}.") : Ok(products);
        }


        [HttpGet("[action]/{Product_Id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetProductInventoryLogs(int Product_Id)
        {
            var product = await Service.GetOneProductAsync(Product_Id);
            if (product == null) return NotFound($"No product found for this Id: {Product_Id}.");

            var logs = await Service.GetProductInventoryLogsAsync(Product_Id);
            return logs == null ? NotFound($"No logs found for this product Id: {Product_Id}.") : Ok(logs);
        }

        [HttpGet("[action]/{Product_Id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetProductSalesReport(int Product_Id)
        {
            try
            {
                var report = await Service.GetProductSalesReportAsync(Product_Id);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("[action]/{Category_Id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetCategorySalesReport(int Category_Id)
        {
            try
            {
                var reports = await Service.GetCategorySalesReportAsync(Category_Id);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("[action]/{Category_id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetCategoryInventoryLogs(int Category_id)
        {
            var category = await Service.GetOneCategoryAsync(Category_id);
            if(category == null) return NotFound($"No category found for this {Category_id}.");

            var products = await Service.GetCategoryProductsAsync(Category_id);
            if(products == null) return NotFound($"No Products found for this Category {Category_id}.");

            var logs = await Service.GetCategoryInventoryLogsAsync(Category_id);
            return logs == null ? NotFound("No logs found for this category.") : Ok(logs);

        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> NewProduct(CreateProductDto dto)
        {
            var UserIdString = userManager.GetUserId(User);
            if(UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                var product = await Service.NewProductAsync(dto,UserId);
                return CreatedAtAction
                    (
                        actionName: nameof(GetOneProduct),   // The method to fetch ONE item
                        routeValues: new { id = product.Id }, // The ID generated by the DB
                        value: product.ToManagerProductDto()            // The "Read" version of the new item
                    );
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> NewCategory(CreateCategoryDto dto)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            var category = await Service.NewCategoryAsync(dto, UserId);
            return CreatedAtAction
                (
                    actionName: nameof(GetOneCategory),   // The method to fetch ONE item
                    routeValues: new { id = category.Id }, // The ID generated by the DB
                    value: category.ToCategoryDto()            // The "Read" version of the new item
                );
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AdjustStock(AdjustStockDto dto)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserId = int.Parse(UserIdString);
            try
            {
                await Service.AdjustStock(dto.ProductId, dto.ChangeAmount, dto.Reason,UserId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NewOrder(CreateOrderDto dto)
        {
            var userIdString = userManager.GetUserId(User);
            if (userIdString == null) return Unauthorized();

            int userId = int.Parse(userIdString);
            try
            {
                
                var order = await Service.NewOrderAsync(dto, userId);
                return CreatedAtAction
                    (
                        actionName: nameof(GetOneOrder),   // The method to fetch ONE item
                        routeValues: new { id = order.Id }, // The ID generated by the DB
                        value: order.ToOrderDto()            // The "Read" version of the new item
                    );
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPost("[action]/{OrderId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ShipOrder(int OrderId)
        {
            var UserIdString = userManager.GetUserId(User);
            if(UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);

            try
            {
                var order = await Service.ShipOrderAsync(OrderId, UserID);
                return Ok(order);         // The "Read" version of the new item

        }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

}

        [HttpPost("[action]/{OrderId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeliverOrder(int OrderId)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            { 
                var order = await Service.DeliverOrderAsync(OrderId, UserID);
                return Ok(order);         // The "Read" version of the new item
                    
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        
        [HttpPost("[action]/{OrderId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> PayOrder(int OrderId)

        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
           
            try
            { 
                var order = await Service.PayOrderAsync(OrderId, UserID);
                return Ok(order);       
                    
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        
        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> updateProduct(UpdateProductDto dto)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var product = await Service.UpdateProductAsync(dto, UserID);
                return Ok(product.ToManagerProductDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> updateCategory(CreateCategoryDto dto, [FromRoute] int id)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var category = await Service.UpdateCategoryAsync(dto, id,UserID);
                return Ok(category.ToCategoryDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPatch("[action]/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> updateProductPatch(JsonPatchDocument<UpdateProductDto> patch, [FromRoute] int id)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var product = await Service.UpdateProductPatchAsync(patch, id,UserID);
                return Ok(product.ToManagerProductDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPatch("[action]/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> updateCategoryPatch(JsonPatchDocument<CreateCategoryDto> patch, [FromRoute] int id)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                var category = await Service.UpdateCategoryPatchAsync(patch, id,UserID);
                return Ok(category.ToCategoryDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                await Service.DeleteProductAsync(id,UserID);
                return Ok(new { Message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var UserIdString = userManager.GetUserId(User);
            if (UserIdString == null) return Unauthorized();
            var UserID = int.Parse(UserIdString);
            try
            {
                await Service.DeleteCategoryAsync(id,UserID);
                return Ok(new { Message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        /************************************************************************************************************************/




    }
}
