using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        private readonly ILogger _logger;

        public EmployeesClient(HttpClient httpClient, ILogger<EmployeesClient> logger) 
            : base(httpClient, ServiceAddress.Employees)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetAsync()
        {
            logInfo("Чтение данных о всех сотрудниках...");
            var result = await GetAsync<IEnumerable<Employee>>();
            logInfo("Чтение данных о всех сотрудниках - завершено");

            return result;
        }

        public async Task<Employee> GetAsync(int id)
        {
            logInfo($"Получаем данные для сотрудника ID = {id}...");
            var result = await GetAsync<Employee>(id);
            logInfo($"Получаем данные для сотрудника ID = {id} - завершено");

            return result;
        }

        public async Task<int> AddAsync(Employee employee)
        {
            logInfo($"Добавление нового сотрудника {employee.LastName} {employee.FirstName} {employee.Patronymic}...");
            var response = await PostAsync(employee);

            if (!response.IsSuccessStatusCode)
            {
                logWarn($"Добавление нового сотрудника {employee.LastName} {employee.FirstName} {employee.Patronymic} - ошибка {response.StatusCode}");
                return -1;
            }

            logInfo($"Добавление нового сотрудника {employee.LastName} {employee.FirstName} {employee.Patronymic} - завершено");
            var result = await response.Content.ReadAsAsync<int>();
            logInfo($"Добавление нового сотрудника {employee.LastName} {employee.FirstName} {employee.Patronymic} - присвоен ID = {result}");

            return result;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            logInfo($"Редактирование сотрудника ID = {employee.Id}: {employee.LastName} {employee.FirstName} {employee.Patronymic}...");
            var response = await PutAsync(employee);

            if (response.IsSuccessStatusCode)
                logInfo($"Редактирование сотрудника ID = {employee.Id}: {employee.LastName} {employee.FirstName} {employee.Patronymic} - завершено");
            else
                logWarn($"Редактирование сотрудника ID = {employee.Id}: {employee.LastName} {employee.FirstName} {employee.Patronymic} - ошибка {response.StatusCode}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            logInfo($"Удаление сотрудника ID = {id}...");
            var response = await DeleteByIdAsync(id);
            if (response.IsSuccessStatusCode)
                logInfo($"Удаление сотрудника ID = {id} - завершено");
            else
                logWarn($"Удаление сотрудника ID = {id} - ошибка {response.StatusCode}");

            return response.IsSuccessStatusCode;
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
