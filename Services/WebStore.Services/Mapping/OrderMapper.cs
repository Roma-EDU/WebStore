using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTOs.Orders;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Services.Mapping
{
    public static class OrderMapper
    {
        public static OrderItemDto ToDto(this OrderItem model)
            => model is null ? null
                             : new OrderItemDto(model.Id, model.Price, model.Quantity);

        public static IEnumerable<OrderItemDto> ToDto(this IEnumerable<OrderItem> models) => models?.Select(ToDto);

        public static OrderDto ToDto(this Order model) 
            => model is null ? null
                             : new OrderDto(model.Id, model.Name, model.Phone, model.Address, model.Date, model.Items.ToDto());

        public static IEnumerable<OrderDto> ToDto(this IEnumerable<Order> models) => models?.Select(ToDto);

    }
}
