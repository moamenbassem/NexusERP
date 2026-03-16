using MyERP.Application.Modules.CRM.DTOs;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyERP.Application.Modules.CRM.Mappers
{
    public static class CRMMappers
    {
        private static string GetCustomerStatus(decimal ltv, DateTime? lastPurchase)
        {
            if (ltv > 50000) return ("VIP");

            if (!lastPurchase.HasValue) return ("Lead");

            var daysSinceLastPurchase = (DateTime.UtcNow - lastPurchase.Value).TotalDays;

            if (daysSinceLastPurchase > 180) return ("Inactive");
            if (daysSinceLastPurchase > 90) return ("At Risk");

            return ("Active");
        }
        public static CustomerDetailedHistoryDto ToDetailedHistoryDto(this Customer customer)
        {
            var customerHistoryDto = new CustomerDetailedHistoryDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                LifetimeValue = customer.LifetimeValue,
                LastPurchaseDate = customer.Orders?.Where(x=>x.Status == Domain.Entities.Inventory.OrderStatus.Paid).OrderByDescending(x => x.PaidAt).Select(o => o.PaidAt).FirstOrDefault()
            };
            var segment = GetCustomerStatus(customer.LifetimeValue, customerHistoryDto.LastPurchaseDate);
            customerHistoryDto.Segment = segment;

            if (customer.Interactions != null)
            {
                foreach (var interaction in customer.Interactions)
                {
                    var TimeLineDto = new TimelineItemDto
                    {
                        id = interaction.Id,
                        Date = interaction.CreatedAt,
                        Type = interaction.Type.ToString(),
                        Description = interaction.Note
                    };
                    customerHistoryDto.Timeline.Add(TimeLineDto);
                }
            }
            //if (customer.Orders != null)
            //{
            //    foreach (var order in customer.Orders)
            //    {
            //        customerHistoryDto.Timeline.Add(new TimelineItemDto
            //        {
            //            Date = order.CreatedAt,
            //            Type = "Order",
            //            Description = $"Order #{order.Id} placed. Total: {order.TotalCost:C}"
            //        });
            //    }
            //}
            customerHistoryDto.Timeline = customerHistoryDto.Timeline.OrderByDescending(o => o.Date).ToList();
            return customerHistoryDto;
        }
        public static CustomerHistoryDto ToHistoryDto(this Customer customer)
        {
            var customerHistoryDto = new CustomerHistoryDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                LifetimeValue = customer.LifetimeValue,
                LastPurchaseDate = customer.Orders?.Where(x => x.Status == Domain.Entities.Inventory.OrderStatus.Paid).OrderByDescending(x => x.PaidAt).Select(o => o.PaidAt).FirstOrDefault()
            };
            var segment = GetCustomerStatus(customer.LifetimeValue, customerHistoryDto.LastPurchaseDate);
            customerHistoryDto.Segment = segment;


            return customerHistoryDto;
        }

        public static CustomerInteraction ToCustomerInteraction(this CreateCustomerInteractionDto dto, int employeeId)
        {
            return new CustomerInteraction
            {
                EmployeeId = employeeId,
                CustomerId = dto.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Note = dto.Note,
                Type = dto.Type,
            };
        }
        public static CustomerInteractionDto ToCustomerInteractionDto(this CustomerInteraction interaction)
        {
            return new CustomerInteractionDto
            {
                EmployeeId = interaction.EmployeeId,
                CustomerId = interaction.CustomerId,
                Note = interaction.Note,
                Type = interaction.Type
            };
        }
    }
}
