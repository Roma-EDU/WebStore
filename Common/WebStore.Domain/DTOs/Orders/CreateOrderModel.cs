using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTOs.Orders
{
    public record CreateOrderModel(
        OrderViewModel OrderVM, 
        IList<OrderItemDto> Items);
}
