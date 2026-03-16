using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Inventory.DTOs
{
    public class CreateOrderDto
    {
        public IEnumerable<CreateOrderProductDto>? orderProducts { get; set; } = new List<CreateOrderProductDto>();

    }
}
