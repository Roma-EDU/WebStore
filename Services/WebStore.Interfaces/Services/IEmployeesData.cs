using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        Task<IEnumerable<Employee>> GetAsync();

        Task<Employee> GetAsync(int id);

        Task<int> AddAsync(Employee employee);

        Task<bool> UpdateAsync(Employee employee);

        Task<bool> DeleteAsync(int id);
    }
}
