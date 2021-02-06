using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(string brandId)
        {
            var id = int.TryParse(brandId, out var brId) ? brId : (int?)null;
            ViewBag.BrandId = id;
            return View(GetBrands());
        }

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
                .OrderBy(brand => brand.Order)
                .Select(brand => new BrandViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    ProductsCount = brand.ProductsCount
                });
    }
}
