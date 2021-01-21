using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTOs.Orders;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(ServiceAddress.Orders.Name)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<OrderDto> GetOrderById(int id)
        {
            return await _orderService.GetOrderById(id);
        }

        [HttpGet(ServiceAddress.Orders.Users + "/{UserName}")]
        public async Task<IEnumerable<OrderDto>> GetUserOrders(string UserName)
        {
            return await _orderService.GetUserOrders(UserName);
        }

        [HttpPost("{UserName}")]
        public async Task<OrderDto> CreateOrder(string UserName, [FromBody] CreateOrderModel OrderModel)
        {
            return await _orderService.CreateOrder(UserName, OrderModel);
        }
    }
}
