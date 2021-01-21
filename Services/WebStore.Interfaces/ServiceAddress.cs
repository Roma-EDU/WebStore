using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Interfaces
{
    public static class ServiceAddress
    {
        public const string WebApiBaseUrl = "api/";

        public const string Values = WebApiBaseUrl + "values";

        public const string Employees = WebApiBaseUrl + "employees";

        public static class Products
        {
            public const string Name = WebApiBaseUrl + "products";

            public const string Brands = "brands";

            public const string Sections = "sections";
        }

        public static class Orders 
        {
            public const string Name = WebApiBaseUrl + "orders";

            public const string Users = "users";
        }
        
    }
}
