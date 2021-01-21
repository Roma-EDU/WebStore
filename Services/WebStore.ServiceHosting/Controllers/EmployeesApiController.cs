using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(ServiceAddress.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _employeesData;

        public EmployeesApiController(IEmployeesData employeesData)
        {
            _employeesData = employeesData;
        }

        [HttpGet]
        public Task<IEnumerable<Employee>> GetAsync()
        {
            return _employeesData.GetAsync();
        }

        [HttpGet("{id}")]
        public Task<Employee> GetAsync(int id)
        {
            return _employeesData.GetAsync(id);
        }

        [HttpPost]
        public Task<int> AddAsync([FromBody] Employee employee)
        {
            return _employeesData.AddAsync(employee);
        }

        [HttpPut]
        public Task<bool> UpdateAsync([FromBody] Employee employee)
        {
            return _employeesData.UpdateAsync(employee);
        }

        [HttpDelete("{id}")]
        public Task<bool> DeleteAsync(int id)
        {
            return _employeesData.DeleteAsync(id);
        }
    }
}
