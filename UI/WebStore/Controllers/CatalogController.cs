using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using WebStore.Domain.ViewModels;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var pageSize = PageSize ?? (int.TryParse(_configuration["CatalogPageSize"], out var value) ? value : null);
            var pageProducts = _productData.GetProducts(new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = pageSize,
            });

            var model = new CatalogViewModel()
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Products = pageProducts.Products.OrderBy(p => p.Order).ToView(),
                PageViewModel = new PageViewModel
                {
                    PageNumber = Page,
                    PageSize = pageSize ?? 0,
                    TotalItems = pageProducts.TotalCount,
                }
            };
            return View(model);
        }

        public IActionResult Shop2(ProductFilter filter)
        {
            var products = _productData.GetProducts(filter);

            return View("Shop", new CatalogViewModel
            {
                SectionId = filter.SectionId,
                BrandId = filter.BrandId,
                Products = products.Products
                   .OrderBy(p => p.Order)
                   .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl
                    })
            });
        }
        
        public IActionResult Details(int id)
        {
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();
            
            return View(product.ToView());
        }
    }
}
