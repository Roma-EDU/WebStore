using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Interfaces;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient httpClient)
            : base(httpClient, ServiceAddress.Values)
        {
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            return await GetAsync<IEnumerable<string>>();
        }

        public async Task<string> GetAsync(int id)
        {
            return await GetAsync<string>(id);
        }

        public async Task<Uri> PostAsync(string value)
        {
            var result = await PostAsync(value, null);
            return result.EnsureSuccessStatusCode().Headers.Location;
        }

        public async Task<HttpStatusCode> UpdateAsync(int id, string value)
        {
            var result = await PutAsync(value, $"{id}");
            return result.StatusCode;
        }

        async Task<HttpStatusCode> IValuesService.DeleteAsync(int id)
        {
            var result = await DeleteByIdAsync(id);
            return result.StatusCode;
        }
    }
}
