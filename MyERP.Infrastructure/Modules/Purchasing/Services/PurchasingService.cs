using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.Inventory.Interfaces;
using MyERP.Application.Modules.Purcahsing.DTOs;
using MyERP.Application.Modules.Purcahsing.Interfaces;
using MyERP.Application.Modules.Purcahsing.Mappers;
using MyERP.Domain.Entities.Inventory;
using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyERP.Infrastructure.Modules.Purchasing.Services
{
    public class PurchasingService : IPurchasingService
    {
        private readonly IUnitOfWork myunit;
        private readonly IInventoryService inventoryService;
        private readonly IAuditLogService auditLogService;
        public PurchasingService(IUnitOfWork _myunit, IInventoryService _inventoryService, IAuditLogService _auditLogService)
        {
            myunit = _myunit;
            inventoryService = _inventoryService;
            auditLogService = _auditLogService;
        }
        public async Task<SupplierDto> GetOneSupplierAsync(int id)
        {
            var Supplier = await myunit.SupplierRepo.GetByIdAsync(id);
            if (Supplier == null) throw new Exception($"Supplier with Id: {id} is not found");
            return Supplier.ToDto();
        }
        public async Task<IEnumerable<SupplierDto>> GetAllSupplierAsync()
        {
            var Suppliers = await myunit.SupplierRepo.GetAllAsync();
            if (Suppliers == null) throw new Exception($"No Suppliers found");
            var dtos = Suppliers.Select(x => x.ToDto());
            return dtos;
        }
        public async Task<IEnumerable<PurchasingOrderDto>> GetAllPurchasingOrdersAsync()
        {
            var orders = await myunit.PurchasingOrderRepo.GetAllAsync();
            if (orders == null) throw new Exception($"No Suppliers found");
            var dtos = orders.Select(x => x.ToDto());
            return dtos;
        }

        public async Task<SupplierDto> NewSupplierAsync(CreateSupplierDto dto)
        {
            var supplier = dto.ToSupplier();
            await myunit.SupplierRepo.AddAsync(supplier);
            await myunit.Commit();
            return supplier.ToDto();
        }

        public async Task<SupplierDto> UpdateSupplierAsync(CreateSupplierDto dto, int id)
        {
            var Supplier = await myunit.SupplierRepo.GetByIdAsync(id);
            if (Supplier == null) throw new Exception($"Supplier with Id: {id} is not found");

            Supplier.Name = dto.Name;
            Supplier.PhoneNumber = dto.PhoneNumber;
            Supplier.Address = dto.Address;
            Supplier.Email = dto.Email;

            myunit.SupplierRepo.Update(Supplier);
            await myunit.Commit();

            return Supplier.ToDto();

        }
        public async Task<PurchasingOrderDto> UpdateOrderAsync(CreatePurchasingOrderDto CreateDto, int id, int UserId)
        {

            var order = await myunit.PurchasingOrderRepo.GetByIdAsync(id);
            if (order == null) throw new Exception($"Order with Id: {id} is not found");
            if (order.Status == POStatus.Received) throw new Exception("Already received");
            

            if (CreateDto.SupplierId == 0 || CreateDto.SupplierId == null) throw new Exception("A supplier Must be Assigned to the Order");
            var s = await myunit.SupplierRepo.GetByIdAsync(CreateDto.SupplierId);
            if (s == null) throw new Exception($"Cannot Find A Supplier of Id: {CreateDto.SupplierId}");

            if (CreateDto.purchasingOrderProduct == null || !CreateDto.purchasingOrderProduct.Any()) throw new Exception($"Can't Place An order Without Products");

                order.SupplierId = CreateDto.SupplierId;
                order.Status = POStatus.Ordered;
                order.TotalCost = 0;
                order.CreatedAt = DateTime.UtcNow;
                order.purchasingOrderProduct.Clear();
            //var POPs = new List<PurchasingOrderProduct>();
            foreach (var product in CreateDto.purchasingOrderProduct)
            {
                var p = await myunit.ProductRepo.GetByIdAsync(product.ProductId);
                if (p == null) throw new Exception($"Cannot Order A product of Id: {product.ProductId}");
                if (product.Quantity <= 0) throw new Exception($"Quantity of ProductId: {product.ProductId} cannot equal {product.Quantity} ");
                var pop = new PurchasingOrderProduct
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    PurchasingOrderId = order.Id,
                    CostPrice = p.CurrentCostPrice
                };
                order.TotalCost += (pop.CostPrice * pop.Quantity);
                order.purchasingOrderProduct.Add(pop);
                //POPs.Add(pop);
            }
            //order.purchasingOrderProduct = POPs;


            myunit.PurchasingOrderRepo.Update(order);
            await auditLogService.LogAsync(UserId, "Updating Purchasing Order", "PurchasingOrder", order.Id);
            await myunit.Commit();

            return order.ToDto();

        }
        public async Task<SupplierDto> UpdateSupplierPatchAsync(JsonPatchDocument<CreateSupplierDto> PATCH, int id)
        {
            var Supplier = await myunit.SupplierRepo.GetByIdAsync(id);
            if (Supplier == null) throw new Exception($"Supplier with Id: {id} is not found");

            var dto = Supplier.ToCreateSupplierDto();
            
            PATCH.ApplyTo(dto);

            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            if (!isValid)
            {
                // Pick the first error or join them all
                var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new Exception($"Validation failed: {errors}");
            }

            Supplier.Name = dto.Name;
            Supplier.PhoneNumber = dto.PhoneNumber;
            Supplier.Address = dto.Address;
            Supplier.Email = dto.Email;

            myunit.SupplierRepo.Update(Supplier);
            await myunit.Commit();

            return Supplier.ToDto();
        }
        public async Task DeleteSupplierAsync(int id)
        {
            if (id == 4) throw new Exception("The fallback 'Unknown' supplier is a system record and cannot be deleted.");

            var supplier = await myunit.SupplierRepo.GetByIdAsync(id);

            if (supplier == null) throw new Exception($"Supplier with Id: {id} is not found");

            if (supplier.purchasingOrder != null && supplier.purchasingOrder.Any())
            {
                foreach (var order in supplier.purchasingOrder)
                {
                    order.SupplierId = 4;
                }
            }

            myunit.SupplierRepo.Delete(supplier);

            await myunit.Commit();
        }

        public async Task<PurchasingOrderDto> PlaceAnOrderAsync(CreatePurchasingOrderDto CreateDto, int UserId)
        {
            if (CreateDto.SupplierId == 0 || CreateDto.SupplierId == null) throw new Exception("A supplier Must be Assigned to the Order");
            var s = await myunit.SupplierRepo.GetByIdAsync(CreateDto.SupplierId);
            if(s == null) throw new Exception($"Cannot Find A Supplier of Id: {CreateDto.SupplierId}");

            if (CreateDto.purchasingOrderProduct == null || !CreateDto.purchasingOrderProduct.Any()) throw new Exception($"Can't Place An order Without Products");
            var order = new PurchasingOrder
            {
                SupplierId = CreateDto.SupplierId,
                Status = POStatus.Ordered,
                TotalCost = 0
                
            };
            var POPs = new List<PurchasingOrderProduct>();
            foreach(var product in CreateDto.purchasingOrderProduct)
            {
                var p = await myunit.ProductRepo.GetByIdAsync(product.ProductId);
                if (p == null) throw new Exception($"Cannot Order A product of Id: {product.ProductId}");
                if (product.Quantity <= 0) throw new Exception($"Quantity of ProductId: {product.ProductId} cannot equal {product.Quantity} ");
                var pop = new PurchasingOrderProduct
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    PurchasingOrderId = order.Id,
                    CostPrice = p.CurrentCostPrice
                };
                order.TotalCost += (pop.CostPrice * pop.Quantity);
                POPs.Add(pop);
            }
            order.purchasingOrderProduct = POPs;
            
            await myunit.PurchasingOrderRepo.AddAsync(order);
            await myunit.Commit();
            await auditLogService.LogAsync(UserId,"Placing New Purchasing Order", "PurchasingOrder", order.Id);
            await myunit.Commit();

            return order.ToDto();
        }

        public async Task<PurchasingOrderDto> ReceiveOrderAsync(int OrderId,int UserId)
        {
            var order = await myunit.PurchasingOrderRepo.GetByIdAsync(OrderId);
            if (order == null) throw new Exception($"Order of Id:{OrderId} Cannot be Found");
            if (order.Status == POStatus.Received) throw new Exception("Already received");

            foreach(var product in order.purchasingOrderProduct)
            {
                await inventoryService.InternalAdjustStockLogic(product.ProductId, product.Quantity, $"Order {OrderId} recieved");
            }

            order.Status = POStatus.Received;
            order.RecievedAt = DateTime.UtcNow;
            await auditLogService.LogAsync(UserId, "Recieving Purchasing Order", "PurchasingOrder", order.Id);

            await myunit.Commit();
            return order.ToDto();
        }
        public async Task<PurchasingOrderDto> CancelOrderAsync(int OrderId, int UserId)
        {
            var order = await myunit.PurchasingOrderRepo.GetByIdAsync(OrderId);
            if (order == null) throw new Exception($"Order of Id:{OrderId} Cannot be Found");
            if (order.Status == POStatus.Received) throw new Exception("Already received");


            order.Status = POStatus.Cancelled;
            await auditLogService.LogAsync(UserId, "Canceling Purchasing Order", "PurchasingOrder", order.Id);

            await myunit.Commit();
            return order.ToDto();
        }

    }
}
