using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.Interfaces
{
    public interface IInventoryService
    {
        public Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        public Task<IEnumerable<ManagerProductDto>> ManagerGetAllProductsAsync();
        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        public Task<IEnumerable<OrderDto>> GetUserOrderHistoryAsync(int userId);
        public Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        public Task<IEnumerable<InventoryLog>> GetAllInventoryLogsAsync();
        public Task<IEnumerable<ProductDto>> GetCategoryProductsAsync(int Category_id);
        public Task<IEnumerable<InventoryLog>> GetProductInventoryLogsAsync(int Product_Id);
        public Task<IEnumerable<InventoryLog>> GetCategoryInventoryLogsAsync(int Category_id);
        public Task<ProductDto> GetOneProductAsync(int id);
        public Task<CategoryDto> GetOneCategoryAsync(int id);
        public Task<OrderDto> GetOneOrderAsync(int id);
        public Task<ProductSalesReportDto> GetProductSalesReportAsync(int id);
        public Task<IEnumerable<ProductSalesReportDto>> GetCategorySalesReportAsync(int id);
        public Task<Product> NewProductAsync(CreateProductDto dto, int UserId);
        public Task<Category> NewCategoryAsync(CreateCategoryDto dto, int UserId);
        public Task<Order> NewOrderAsync(CreateOrderDto dto, int userId);
        public Task<OrderDto> ShipOrderAsync(int id, int UserId);
        public Task<OrderDto> DeliverOrderAsync(int id, int UserId);
        public Task<OrderDto> PayOrderAsync(int id, int UserId);
        public Task<Product> UpdateProductAsync(UpdateProductDto dto, int UserId);
        public Task<Category> UpdateCategoryAsync(CreateCategoryDto dto, int id, int UserId);
        public Task<Product> UpdateProductPatchAsync(JsonPatchDocument<UpdateProductDto> patch, int id, int UserId);
        public Task<Category> UpdateCategoryPatchAsync(JsonPatchDocument<CreateCategoryDto> patch, int id, int UserId);
        public Task DeleteProductAsync(int id, int UserId);
        public Task DeleteCategoryAsync(int id, int UserId);
        public Task AdjustStock(int ProductId, int ChangeAmount, string Reason, int UserId);
        public Task InternalAdjustStockLogic(int productId, int amount, string reason);

    }
}
