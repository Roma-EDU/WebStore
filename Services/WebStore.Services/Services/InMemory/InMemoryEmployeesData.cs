using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Services.Data;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using System.Threading.Tasks;

namespace WebStore.Services.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly List<Employee> _Employees = TestData.Employees;

        public IEnumerable<Employee> Get() => _Employees;

        public Employee Get(int id) => _Employees.FirstOrDefault(item => item.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee))
                return employee.Id;

            employee.Id = _Employees
               .Select(item => item.Id)
               .DefaultIfEmpty()
               .Max() + 1;

            _Employees.Add(employee);

            return employee.Id;
        }

        public bool Update(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee))
                return true;

            var db_item = Get(employee.Id);
            if (db_item is null)
                return false;

            db_item.LastName = employee.LastName;
            db_item.FirstName = employee.FirstName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;
            return true;
        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item is null) return false;
            return _Employees.Remove(item);
        }

        Task<IEnumerable<Employee>> IEmployeesData.GetAsync()
        {
            return Task.FromResult(Get());
        }

        Task<Employee> IEmployeesData.GetAsync(int id)
        {
            return Task.FromResult(Get(id));
        }

        Task<int> IEmployeesData.AddAsync(Employee employee)
        {
            return Task.FromResult(Add(employee));
        }

        Task<bool> IEmployeesData.UpdateAsync(Employee employee)
        {
            return Task.FromResult(Update(employee));
        }

        Task<bool> IEmployeesData.DeleteAsync(int id)
        {
            return Task.FromResult(Delete(id));
        }
    }
}
