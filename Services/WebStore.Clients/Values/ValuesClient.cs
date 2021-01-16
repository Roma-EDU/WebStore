using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient httpClient)
            : base(httpClient, "api/values")
        {
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = await Http.GetAsync(Address);
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<string>();

            var result = await response.Content.ReadAsAsync<IEnumerable<string>>();
            return result;
        }

        public async Task<string> GetAsync(int id)
        {
            var response = await Http.GetAsync($"{Address}/{id}");
            if (!response.IsSuccessStatusCode)
                return string.Empty;

            return await response.Content.ReadAsAsync<string>();
        }

        public async Task<Uri> PostAsync(string value)
        {
            var response = await Http.PostAsJsonAsync(Address, value);
            return response.EnsureSuccessStatusCode().Headers.Location;
        }

        public async Task<HttpStatusCode> UpdateAsync(int id, string value)
        {
            var response = await Http.PutAsJsonAsync($"{Address}/{id}", value);
            return response.EnsureSuccessStatusCode().StatusCode;
        }
        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            var response = await Http.DeleteAsync($"{Address}/{id}");
            return response.EnsureSuccessStatusCode().StatusCode;
        }

    }
}
