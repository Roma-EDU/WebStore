using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTOs.Products;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient httpClient) 
            : base(httpClient, ServiceAddress.Products.Name)
        {
        }

        public BrandDto GetBrandById(int id)
        {
            return GetAsync<BrandDto>($"{ServiceAddress.Products.Brands}/{id}").Result;
        }

        public IEnumerable<BrandDto> GetBrands()
        {
            return GetAsync<IEnumerable<BrandDto>>($"{ServiceAddress.Products.Brands}").Result;
        }

        public ProductDto GetProductById(int id)
        {
            return GetAsync<ProductDto>(id).Result;
        }

        public PagedProductDto GetProducts(ProductFilter filter = null)
        {
            //В запрос передаётся сложные объект ProductFilter, поэтому вместо Get-запроса 
            //отправляем его в теле запроса методом Post
            //Плюс для случая Null (т.к. не сериализуется) заменяем фильтр на пустой объект
            var response = PostAsync(filter ?? new ProductFilter()).Result;
            return response.EnsureSuccessStatusCode().Content.ReadAsAsync<PagedProductDto>().Result;
        }

        public SectionDto GetSectionById(int id)
        {
            return GetAsync<SectionDto>($"{ServiceAddress.Products.Sections}/{id}").Result;
        }

        public IEnumerable<SectionDto> GetSections()
        {
            return GetAsync<IEnumerable<SectionDto>>($"{ServiceAddress.Products.Sections}").Result;
        }
    }
}
