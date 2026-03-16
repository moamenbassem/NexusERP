using MyERP.Application.Modules.Purcahsing.DTOs;
using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyERP.Application.Modules.Purcahsing.Mappers
{
    public static class PurchasingMappers
    {
        public static SupplierDto ToDto(this Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                Address = supplier.Address

            };
        }
        public static CreateSupplierDto ToCreateSupplierDto(this Supplier supplier)
        {
            return new CreateSupplierDto
            {
                Name = supplier.Name,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                Address = supplier.Address

            };
        }

        public static Supplier ToSupplier(this CreateSupplierDto dto)
        {
            return new Supplier
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address
            };
        }

        public static PurchasingOrderDto ToDto(this PurchasingOrder order)
        {
            var dto = new PurchasingOrderDto
            {
                Id = order.Id,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                SupplierId= order.SupplierId,
                TotalCost= order.TotalCost,
                RecievedAt = order.RecievedAt
            };
            
            var POPDtos = new List<PurchasingOrderProductDto>();
            
            foreach(var op in order.purchasingOrderProduct)
            {
                var purchasingOrderProduct = new PurchasingOrderProductDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    CostPrice = op.CostPrice,
                };
                POPDtos.Add(purchasingOrderProduct);

            }
            dto.purchasingOrderProduct = POPDtos;


            return dto;
        }
    }
}
