using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Interfaces.TestAPI
{
    public interface IValuesService
    {
        Task<IEnumerable<string>> GetAsync();

        Task<string> GetAsync(int id);

        Task<Uri> PostAsync(string value);

        Task<HttpStatusCode> UpdateAsync(int id, string value);

        Task<HttpStatusCode> DeleteAsync(int id);
    }
}
