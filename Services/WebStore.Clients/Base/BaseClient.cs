using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        protected Task<T> GetAsync<T>(string url = null) => GetAsync<T>(url, CancellationToken.None);
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            var response = await HttpClient.GetAsync(buildUrl(url), cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<T>(cancellationToken);
        }

        protected Task<HttpResponseMessage> PostAsync<T>(T value, string url = null) => PostAsync(value, url, CancellationToken.None);
        protected async Task<HttpResponseMessage> PostAsync<T>(T value, string url, CancellationToken cancellationToken)
        {
            return await HttpClient.PostAsJsonAsync<T>(buildUrl(url), value, cancellationToken);
        }

        protected Task<HttpResponseMessage> PutAsync<T>(T value, int id) => PutAsync(value, $"{id}");
        protected Task<HttpResponseMessage> PutAsync<T>(T value, string url = null) => PutAsync(value, url, CancellationToken.None);
        protected async Task<HttpResponseMessage> PutAsync<T>(T value, string url, CancellationToken cancellationToken)
        {
            return await HttpClient.PutAsJsonAsync(buildUrl(url), value, cancellationToken);
        }

        protected Task<HttpResponseMessage> DeleteByIdAsync(int id) => DeleteAsync($"{id}");
        protected Task<HttpResponseMessage> DeleteAsync(string url) => DeleteAsync(url, CancellationToken.None);
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken)
        {
            return await HttpClient.DeleteAsync(buildUrl(url));
        }
    }
}
