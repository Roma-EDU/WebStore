using System;
using System.Collections.Generic;

namespace WebStore.Domain.DTOs.Orders
{
    public record OrderDto(
        int Id, 
        string Name, 
        string Phone, 
        string Address, 
        DateTime Date, 
        IEnumerable<OrderItemDto> Items);
}
