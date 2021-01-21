using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Base;
using WebStore.Domain.DTOs.Orders;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        private readonly ILogger _logger;

        public OrdersClient(HttpClient httpClient, ILogger<OrdersClient> logger) 
            : base(httpClient, ServiceAddress.Orders.Name)
        {
            _logger = logger;
        }

        public async Task<OrderDto> GetOrderById(int id)
        {
            return await GetAsync<OrderDto>(id);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrders(string UserName)
        {
            return await GetAsync<IEnumerable<OrderDto>>($"{ServiceAddress.Orders.Users}/{UserName}");
        }

        public async Task<OrderDto> CreateOrder(string UserName, CreateOrderModel OrderModel)
        {
            var itemsCount = OrderModel?.Items?.Count ?? 0;
            logInfo($"Создание нового заказа для пользователя {UserName} ({OrderModel.OrderVM.Phone}) c {itemsCount} позициями...");
            var response = await PostAsync(OrderModel, UserName);
            if (!response.IsSuccessStatusCode)
            {
                logWarn($"Создание нового заказа для пользователя {UserName} ({OrderModel.OrderVM.Phone}) c {itemsCount} позициями - ошибка {response.StatusCode}");
                return null;
            }

            logInfo($"Создание нового заказа для пользователя {UserName} ({OrderModel.OrderVM.Phone}) c {itemsCount} позициями - завершено");
            return await response.Content.ReadAsAsync<OrderDto>();
        }

        private void logInfo(string message)
        {
            _logger.LogInformation(message);
        }

        private void logWarn(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
