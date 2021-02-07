using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //Комнпонент "Хлебные крошки", показывающий где мы находимся
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData)
        {
            _productData = productData;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new BreadCrumbsViewModel();

            //Параметры секции и бренда лежат в строке запроса
            if (int.TryParse(Request.Query["SectionId"], out var sectionId))
            {
                viewModel.Section = _productData.GetSectionById(sectionId);
                var parentSectionId = viewModel.Section?.ParentId;
                if (parentSectionId != null)
                    viewModel.MasterSection = _productData.GetSectionById(parentSectionId.Value);
            }

            if (int.TryParse(Request.Query["BrandId"], out var brandId))
                viewModel.Brand = _productData.GetBrandById(brandId);

            //Информация о товаре лежат в другом месте: в маршруте
            if (int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var productId))
                viewModel.Product = _productData.GetProductById(productId)?.Name;

            return View(viewModel);
        }
    }
}
