using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected BaseClient(HttpClient httpClient, string serviceAddress)
        {
            HttpClient = httpClient;
            Address = serviceAddress;
        }

        protected string Address { get; }

        protected HttpClient HttpClient { get; }

        private string buildUrl(string url = null)
        {
            if (string.IsNullOrEmpty(url))
                return Address;

            return $"{Address}/{url}";
        }

        protected async Task<T> GetAsync<T>(string url = null)
        {
            var response = await HttpClient.GetAsync(buildUrl(url));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<T>();
            return result;
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(T value, string url = null)
        {
            var response = await HttpClient.PostAsJsonAsync<T>(buildUrl(url), value);
            return response.EnsureSuccessStatusCode();
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(T value, string url = null)
        {
            var response = await HttpClient.PutAsJsonAsync(buildUrl(url), value);
            return response.EnsureSuccessStatusCode();
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await HttpClient.DeleteAsync(buildUrl(url));
            return response.EnsureSuccessStatusCode();
        }
    }
}
