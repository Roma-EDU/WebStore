using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(ServiceAddress.Products.Name)]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet(ServiceAddress.Products.Brands + "/{id}")]
        public BrandDto GetBrandById(int id)
        {
            return _productData.GetBrandById(id);
        }

        [HttpGet(ServiceAddress.Products.Brands)]
        public IEnumerable<BrandDto> GetBrands()
        {
            return _productData.GetBrands();
        }

        [HttpGet("{id}")]
        public ProductDto GetProductById(int id)
        {
            return _productData.GetProductById(id);
        }

        [HttpPost]
        public IEnumerable<ProductDto> GetProducts([FromBody] ProductFilter Filter = null)
        {
            return _productData.GetProducts(Filter);
        }

        [HttpGet(ServiceAddress.Products.Sections + "/{id}")]
        public SectionDto GetSectionById(int id)
        {
            return _productData.GetSectionById(id);
        }

        [HttpGet(ServiceAddress.Products.Sections)]
        public IEnumerable<SectionDto> GetSections()
        {
            return _productData.GetSections();
        }
    }
}
