using Microsoft.AspNetCore.JsonPatch;
using MyERP.Application.Modules.Purcahsing.DTOs;
using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Purcahsing.Interfaces
{
    public interface IPurchasingService
    {
        public Task<SupplierDto> GetOneSupplierAsync(int id);
        public Task<IEnumerable<SupplierDto>> GetAllSupplierAsync();
        public Task<IEnumerable<PurchasingOrderDto>> GetAllPurchasingOrdersAsync();
        public Task<SupplierDto> NewSupplierAsync(CreateSupplierDto dto);
        public Task<SupplierDto> UpdateSupplierAsync(CreateSupplierDto dto, int id);
        public Task<PurchasingOrderDto> UpdateOrderAsync(CreatePurchasingOrderDto CreateDto, int id, int UserId);
        public Task<SupplierDto> UpdateSupplierPatchAsync(JsonPatchDocument<CreateSupplierDto> PATCH, int id);
        public Task DeleteSupplierAsync(int id);
        public Task<PurchasingOrderDto> PlaceAnOrderAsync(CreatePurchasingOrderDto CreateDto, int UserId);
        public Task<PurchasingOrderDto> ReceiveOrderAsync(int OrderId, int UserId);
        public Task<PurchasingOrderDto> CancelOrderAsync(int OrderId, int UserId);



    }
}
