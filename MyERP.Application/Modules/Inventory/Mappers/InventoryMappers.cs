using MyERP.Application.Modules.Inventory.DTOs;
using MyERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.Mappers
{
    public static class InventoryMappers
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Name = product.Name,
                Id = product.Id,
                SKU = product.SKU,
                Quantity = product.Quantity,
                CurrentPrice = product.CurrentPrice,
                CreatedAt = product.CreatedAt,
                CategoryId = product.CategoryId
            };
        }
        public static ManagerProductDto ToManagerProductDto(this Product product)
        {
            return new ManagerProductDto
            {
                Name = product.Name,
                Id = product.Id,
                SKU = product.SKU,
                Quantity = product.Quantity,
                CurrentPrice = product.CurrentPrice,
                CreatedAt = product.CreatedAt,
                CategoryId = product.CategoryId,
                CostPrice = product.CurrentCostPrice
            };
        }
        public static UpdateProductDto ToUpdateProductDto(this Product product)
        {
            return new UpdateProductDto
            {
                Name = product.Name,
                SKU = product.SKU,
                Price = product.CurrentPrice,
                CategoryId = product.CategoryId,
                CostPrice = product.CurrentCostPrice
            };
        }
        public static Product toProduct(this CreateProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Quantity = dto.InitialQuantity,
                CurrentPrice = dto.Price,
                SKU = dto.SKU,
                CategoryId = dto.CategoryId,
                CurrentCostPrice = dto.CostPrice
                
            };
        }


        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Name = category.Name,
                Id = category.Id,

            };
        }
        public static CreateCategoryDto ToCreateCategoryDto(this Category category)
        {
            return new CreateCategoryDto
            {
                Name = category.Name
            };
        }

        public static OrderDto ToOrderDto(this Order order)
        {
            var dto = new OrderDto()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status,
                DeliveredAt = order.DeliveredAt,
                CreatedAt = order.CreatedAt,
                PaidAt = order.PaidAt,
                TotalCost = order.TotalCost
            };

            var ops = new List<OrderProductDto>();
            foreach (var op in order.orderProduct)
            {
                var orderproduct = new OrderProductDto
                {
                    ProductId = op.ProductId,
                    Qty = op.Qty,
                    Price = op.Price
                };
                ops.Add(orderproduct);
            }
            dto.orderProducts = ops;

            return dto;
        }




    }
}
