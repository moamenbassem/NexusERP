using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.CRM.DTOs;
using MyERP.Application.Modules.CRM.Interfaces;
using MyERP.Application.Modules.Finance.Interfaces;
using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Application.Modules.Inventory.Interfaces;
using MyERP.Application.Modules.Inventory.Mappers;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.Finance;
using MyERP.Domain.Entities.Identity;
using MyERP.Domain.Entities.Inventory;
using MyERP.Infrastructure.Data;
using MyERP.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace MyERP.Infrastructure.Modules.Inventory.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork myunit;
        private readonly IAuditLogService auditLogService;
        private readonly IFinanceService financeService;
        private readonly ICRMService crmService;
        public InventoryService(IUnitOfWork _myunit, IAuditLogService _auditLogService,IFinanceService _financeService, ICRMService _crmService)
        {
            myunit = _myunit;
            auditLogService = _auditLogService;
            financeService = _financeService;
            crmService = _crmService;
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await myunit.ProductRepo.GetAllAsync();
            if (products == null || !products.Any()) return null;
            var dtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var dto = product.ToProductDto();
                dtos.Add(dto);
            }

            return dtos;
        }
        public async Task<IEnumerable<ManagerProductDto>> ManagerGetAllProductsAsync()
        {
            var products = await myunit.ProductRepo.GetAllAsync();
            if (products == null || !products.Any()) return null;
            var dtos = new List<ManagerProductDto>();
            foreach (var product in products)
            {
                var dto = product.ToManagerProductDto();
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await myunit.CategoryRepo.GetAllAsync();
            if (categories == null || !categories.Any()) return null;
            var dtos = new List<CategoryDto>();
            foreach (var category in categories)
            {
                var dto = category.ToCategoryDto();
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrderHistoryAsync(int userId)
        {
            // Fetch all orders for this user, including their items
            var orders = await myunit.OrderRepo.FindAsync(x=> x.CustomerId == userId);
            if (orders == null || !orders.Any()) throw new Exception("No Orders");

            return orders.Select(o => o.ToOrderDto());
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await myunit.OrderRepo.GetAllAsync();
            if (orders == null || !orders.Any()) return null;
            var dtos = new List<OrderDto>();
            foreach(var order in orders)
            {
                var dto = order.ToOrderDto();
                dtos.Add(dto);
            }
            return dtos;
        }
        public async Task<IEnumerable<InventoryLog>> GetAllInventoryLogsAsync()
        {
            var logs = await myunit.InventoryLogRepo.GetAllAsync();
            if (logs == null || !logs.Any()) return null;
            return logs;
        }

        public async Task<IEnumerable<ProductDto>> GetCategoryProductsAsync(int Category_id)
        {

            var products = await myunit.ProductRepo.FindAsync(x => x.CategoryId == Category_id);
            if (products == null || !products.Any()) return null;
            var dtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var dto = product.ToProductDto();
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<InventoryLog>> GetProductInventoryLogsAsync(int Product_Id)
        {
            var logs = await myunit.InventoryLogRepo.FindAsync(x => x.ProductId == Product_Id);
            if (logs == null || !logs.Any()) return null;
            return logs;
        }

        public async Task<IEnumerable<InventoryLog>> GetCategoryInventoryLogsAsync(int Category_id)
        {
            var products = await myunit.ProductRepo.FindAsync(x => x.CategoryId == Category_id);

            var ids = new List<int>();
            foreach (var product in products)
            {
                ids.Add(product.Id);
            }
            var logs = new List<InventoryLog>();
            foreach (var id in ids)
            {
                var log = await myunit.InventoryLogRepo.FindAsync(x => x.ProductId == id);
                logs.AddRange(log);
            }
            if (logs == null || !logs.Any()) return null;

            return logs;
        }

        public async Task<ProductDto> GetOneProductAsync(int id)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(id);
            if (product == null) return null;
            product.ToProductDto();

            return product.ToProductDto();
        }
        public async Task<CategoryDto> GetOneCategoryAsync(int id)
        {
            var category = await myunit.CategoryRepo.GetByIdAsync(id);
            if (category == null) return null;
            return category.ToCategoryDto();
        }

        public async Task<OrderDto> GetOneOrderAsync(int id)
        {
            var order = await myunit.OrderRepo.GetByIdAsync(id);
            if (order == null) return null;
            if (!order.orderProduct.Any()) return null;
            return order.ToOrderDto();
        }
        public async Task<ProductSalesReportDto> GetProductSalesReportAsync(int id)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(id);
            if (product == null)
                throw new Exception($"No Product of this Id: {id}");
            if (!product.orderProduct.Any()) 
                return new ProductSalesReportDto { ProductId= product.Id, TotalSoldUnits = 0, TotalRevenue = 0 , Profit = 0};
            var Report = new ProductSalesReportDto
            {
                ProductId = product.Id,
                TotalSoldUnits = 0,
                TotalRevenue = 0,
                Profit = 0
            };
            foreach(var op in product.orderProduct)
            {
                Report.TotalSoldUnits += op.Qty;
                Report.TotalRevenue += (op.Qty * op.Price);
                Report.Profit += op.Qty * (op.Price - op.CostPrice);
            }
            return Report;
        }
        public async Task<IEnumerable<ProductSalesReportDto>> GetCategorySalesReportAsync(int id)
        {
            var Products = await myunit.ProductRepo.FindAsync(x=> x.CategoryId == id);
            if (Products == null || !Products.Any())
                throw new Exception($"No Products in this Category of Id: {id}");
            var Reports = new List<ProductSalesReportDto>();
           foreach(var product in Products)
            {
                if (!product.orderProduct.Any())
                {
                    Reports.Add(new ProductSalesReportDto { ProductId = product.Id, TotalSoldUnits = 0, TotalRevenue = 0 });
                    continue;
                }
                var Report = new ProductSalesReportDto
                {
                    ProductId = product.Id,
                    TotalSoldUnits = 0,
                    TotalRevenue = 0,
                    Profit = 0
                };
                foreach (var op in product.orderProduct)
                {
                    Report.TotalSoldUnits += op.Qty;
                    Report.TotalRevenue += (op.Qty * op.Price);
                }
                Reports.Add(Report);
            }

            return Reports;
        }


        public async Task<Product> NewProductAsync(CreateProductDto dto, int UserId)
        {
            if (dto.Price <= 0) throw new Exception($"Price Cannot = {dto.Price}.");
                    
            var category = await myunit.CategoryRepo.GetByIdAsync(dto.CategoryId.Value);
            
            if(category == null)
                throw new Exception($"No Category of this Id: {dto.CategoryId}");

            var product = dto.toProduct();

            await myunit.ProductRepo.AddAsync(product);
            await myunit.Commit();
            await auditLogService.LogAsync(UserId,"Adding New Product","Product",product.Id);
            await myunit.Commit();
            return product;
        }
        public async Task<Category> NewCategoryAsync(CreateCategoryDto dto, int UserId)
        {
            var category = new Category { Name = dto.Name };
            await myunit.CategoryRepo.AddAsync(category);
            await myunit.Commit();
            await auditLogService.LogAsync(UserId, "Adding New Category", "Category", category.Id);
            await myunit.Commit();
            return category;
        }

        public async Task<Order> NewOrderAsync(CreateOrderDto dto, int userId)
        {
            if (dto.orderProducts == null)
                throw new Exception($"No products Selected");

            var order = new Order();
            var Orders = new List<OrderProduct>();
            decimal totalcost = 0;
            foreach (var product in dto.orderProducts)
            {

                var p = await myunit.ProductRepo.GetByIdAsync(product.ProductId);
                if (p == null)
                    throw new Exception($"No Product of this Id: {product.ProductId}");
                if(product.Qty == 0)
                    throw new Exception($"Quantity of product: {p.Name} with Product_Id:{product.ProductId}  Cannot be zero ");

                if ((p.Quantity - product.Qty < 0)) 
                    throw new Exception($"Insufficient stock for product: {p.Name} with Product_Id:{product.ProductId} Available: {p.Quantity}");

                await InternalAdjustStockLogic(p.Id, -product.Qty, "Sold");
                var op = new OrderProduct
                {
                    ProductId = product.ProductId,
                    Qty = product.Qty,
                    OrderId = order.Id,
                    Price = p.CurrentPrice,
                    CostPrice = p.CurrentCostPrice
                };
                totalcost += (op.Price * op.Qty);
                Orders.Add(op);
            }
            order.orderProduct = Orders;
            order.TotalCost = totalcost;
            order.CustomerId = userId;

            await myunit.OrderRepo.AddAsync(order);
            var interaction = new CreateCustomerInteractionDto
            {
                CustomerId = order.CustomerId,
                Note = $"Customer of Id:{order.CustomerId} has ordered new order ",
                Type = InteractionType.Note
            };

            await crmService.AutoLogCustomerIntearactionAsync(interaction);
            await myunit.Commit();

            return order;

        }
        public async Task<OrderDto> ShipOrderAsync(int id, int UserId) 
        {
            var order = await myunit.OrderRepo.GetByIdAsync(id);
            if (order == null) throw new Exception($"Order Of Id {id} Cannot be found");
            if (order.Status == OrderStatus.Shipped)
                throw new Exception("Order is already Shipped.");
            if (order.Status == OrderStatus.Delivered)
                throw new Exception("Order is already delivered.");
            order.Status= OrderStatus.Shipped;

            myunit.OrderRepo.Update(order);
            await financeService.GenerateInvoiceInternalAsync(order);
            await auditLogService.LogAsync(UserId, "Shipping Order and Generating Invoice", "Order", order.Id);

            await myunit.Commit();
            return order.ToOrderDto();

        }
        public async Task<OrderDto> DeliverOrderAsync(int id, int UserId) 
        {


            var order = await myunit.OrderRepo.GetByIdAsync(id);
            if (order == null) throw new Exception($"Order Of Id {id} Cannot be found");

            if (order.Status == OrderStatus.Delivered)
                throw new Exception("Order is already delivered.");

            if (order.Status != OrderStatus.Shipped)
                throw new Exception("Order must be Shipped before it can be Delivered.");
            order.Status = OrderStatus.Delivered;
            order.DeliveredAt = DateTime.UtcNow;

            myunit.OrderRepo.Update(order);
            await auditLogService.LogAsync(UserId, "Order is Delivered", "Order", order.Id);
            var interaction = new CreateCustomerInteractionDto
            {
                CustomerId = order.CustomerId,
                Note = $"Customer of Id:{order.CustomerId} has recieved order of Id: {order.Id}",
                Type = InteractionType.Note
            };

            await crmService.LogCustomerIntearactionAsync(interaction, UserId);
            await myunit.Commit();
            return order.ToOrderDto();
        }
        public async Task<OrderDto> PayOrderAsync(int id, int UserId) 
        {
            var order = await myunit.OrderRepo.GetByIdAsync(id);
            if (order == null) throw new Exception($"Order Of Id {id} Cannot be found");
            
            if (order.Status == OrderStatus.Paid)
                throw new Exception("Order is already paid.");

            if (order.Status != OrderStatus.Delivered)
                throw new Exception("Order must be delivered before payment.");

            if (order.Invoice == null)
                throw new Exception("No invoice found for this order. Did you ship it yet?");

            var invoiceId = order.Invoice.Id;

            await financeService.ProcessPaymentAsync(invoiceId, UserId);
            order.Status = OrderStatus.Paid;
            order.Customer.LifetimeValue += order.TotalCost; 
            order.PaidAt = DateTime.UtcNow;
            myunit.OrderRepo.Update(order);
            await auditLogService.LogAsync(UserId, "Order is Paid", "Order", order.Id);
            var interaction = new CreateCustomerInteractionDto
            {
                CustomerId = order.CustomerId,
                Note = $"Customer of Id:{order.CustomerId} paid order of Id{order.Id}",
                Type = InteractionType.Note
            };

            await crmService.LogCustomerIntearactionAsync(interaction, UserId);
            await myunit.Commit();
            return order.ToOrderDto();
        }
        public async Task<Product> UpdateProductAsync(UpdateProductDto dto,int UserId)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(dto.ProductId);
            if (product == null) throw new Exception($"No product found for this Id: {dto.ProductId}.");

            var Category = await myunit.CategoryRepo.GetByIdAsync(dto.CategoryId.Value);
            if (Category == null) throw new Exception($"No Category found for this Id: {dto.CategoryId}.");

            product.Name = dto.Name;
            product.SKU = dto.SKU;
            product.CurrentPrice = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.CurrentCostPrice = dto.CostPrice;

            myunit.ProductRepo.Update(product);
            await auditLogService.LogAsync(UserId, "Product is Updated", "Product", product.Id);
            await myunit.Commit();
            return product;
        }

        public async Task<Category> UpdateCategoryAsync(CreateCategoryDto dto, int id,int UserId)
        {
            var category = await myunit.CategoryRepo.GetByIdAsync(id);
            if (category == null) throw new Exception($"No Category found for this Id: {id}.");
            category.Name = dto.Name;


            myunit.CategoryRepo.Update(category);
            await auditLogService.LogAsync(UserId, "Category is Updated", "Category", category.Id);

            await myunit.Commit();
            return category;
        }

        public async Task<Product> UpdateProductPatchAsync(JsonPatchDocument<UpdateProductDto> patch, int id,int UserId)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(id);
            if (product == null) throw new Exception ($"No Product found for this Id: {id}.");
            var dto = product.ToUpdateProductDto();


            patch.ApplyTo(dto);
            var category = await myunit.CategoryRepo.GetByIdAsync(dto.CategoryId.Value);
            if (category == null) throw new Exception($"No Category found for this Id: {dto.CategoryId}.");
            if (dto.Price <= 0) throw new Exception($"Price Cannot = {dto.Price}.");

            product.Name = dto.Name;
            product.SKU = dto.SKU;
            product.CurrentPrice = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.CurrentCostPrice = dto.CostPrice;

            myunit.ProductRepo.Update(product);
            await auditLogService.LogAsync(UserId, "Product is Updated", "Product", product.Id);
            await myunit.Commit();
            return product;

        }
        public async Task<Category> UpdateCategoryPatchAsync(JsonPatchDocument<CreateCategoryDto> patch, int id,int UserId)
        {
            var category = await myunit.CategoryRepo.GetByIdAsync(id);
            if (category == null) throw new Exception ($"No Category found for this Id: {id}.");
           
            var dto = category.ToCreateCategoryDto();
            patch.ApplyTo(dto);

            category.Name = dto.Name;
            myunit.CategoryRepo.Update(category);
            await auditLogService.LogAsync(UserId, "Category is Updated", "Category", category.Id);
            await myunit.Commit();
            return category;

        }
        public async Task DeleteProductAsync(int id, int UserId)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(id);
            if (product == null) throw new Exception($"No Product found for this Id: {id}.");

            await auditLogService.LogAsync(UserId, "Product is Deleted", "Product", product.Id);
            myunit.ProductRepo.Delete(product);
            await myunit.Commit();
        }
        public async Task DeleteCategoryAsync(int id, int UserId)
        {
            var category = await myunit.CategoryRepo.GetByIdAsync(id);
            if (category == null) throw new Exception($"No Category found for this Id: {id}.");

            var products = await myunit.ProductRepo.FindAsync(x => x.CategoryId == id);
            foreach (var product in products)
            {
                product.CategoryId = null;
                product.category = null;
            }
            await auditLogService.LogAsync(UserId, "Category is Deleted", "Category", category.Id);
            myunit.CategoryRepo.Delete(category);
            await myunit.Commit();
        }

        public async Task AdjustStock(int ProductId, int ChangeAmount, string Reason, int UserId)
        {
            await InternalAdjustStockLogic(ProductId, ChangeAmount, Reason);
            await auditLogService.LogAsync(UserId, "Adjusting product stock", "Product", ProductId,Reason);

            await myunit.Commit();

        }
        public async Task InternalAdjustStockLogic(int productId, int amount, string reason)
        {
            var product = await myunit.ProductRepo.GetByIdAsync(productId);
            if (product == null) throw new Exception("Product not found");

            product.Quantity += amount;
            var log = new InventoryLog
            {
                ProductId = productId,
                ChangeAmount = amount,
                Note = reason
            };
            await myunit.InventoryLogRepo.AddAsync(log);


            // NO COMMIT HERE
        }
    }
}
