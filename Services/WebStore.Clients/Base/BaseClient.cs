using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected BaseClient(HttpClient httpClient, string serviceAddress)
        {
            Http = httpClient;
            Address = serviceAddress;
        }

        protected string Address { get; }

        protected HttpClient Http { get; }
    }
}
