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

        protected Task<T> GetAsync<T>(int id) => GetAsync<T>($"{id}");
        protected async Task<T> GetAsync<T>(string url = null)
        {
            var response = await HttpClient.GetAsync(buildUrl(url));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<T>();
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(T value, string url = null)
        {
            return await HttpClient.PostAsJsonAsync<T>(buildUrl(url), value);
        }

        protected Task<HttpResponseMessage> PutAsync<T>(T value, int id) => PutAsync(value, $"{id}");

        protected async Task<HttpResponseMessage> PutAsync<T>(T value, string url = null)
        {
            return await HttpClient.PutAsJsonAsync(buildUrl(url), value);
        }

        protected Task<HttpResponseMessage> DeleteByIdAsync(int id) => DeleteAsync($"{id}");
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await HttpClient.DeleteAsync(buildUrl(url));
        }
    }
}
