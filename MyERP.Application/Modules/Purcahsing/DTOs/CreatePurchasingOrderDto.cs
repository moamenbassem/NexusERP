using MyERP.Domain.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Purcahsing.DTOs
{
    public class CreatePurchasingOrderDto
    {
        public int SupplierId { get; set; }
        public IEnumerable<CreatePurchasingOrderProductDto>? purchasingOrderProduct { get; set; }


    }
}
