using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
    public class SiteMapController : Controller
    {
        public IActionResult Index([FromServices] IProductData productData)
        {
            //Статическое содержимое
            var nodes = new List<SitemapNode>()
            {
                new SitemapNode(Url.Action(nameof(HomeController.Index), "Home")),
                new SitemapNode(Url.Action(nameof(HomeController.ContactUs), "Home")),
                new SitemapNode(Url.Action(nameof(HomeController.Blogs), "Home")),
                new SitemapNode(Url.Action(nameof(HomeController.BlogSingle), "Home")),
                new SitemapNode(Url.Action(nameof(CatalogController.Shop), "Catalog")),
                new SitemapNode(Url.Action(nameof(WebAPIController.Index), "WebAPI")),
            };

            //Динамическое содержимое сайта, параметр добавлен с помощью анонимного класса
            nodes.AddRange(productData.GetSections().Select(s => new SitemapNode(Url.Action(nameof(CatalogController.Shop), "Catalog", new { SectionId = s.Id }))));
            nodes.AddRange(productData.GetBrands().Select(b => new SitemapNode(Url.Action(nameof(CatalogController.Shop), "Catalog", new { BrandId = b.Id }))));
            
            //Id продукта записан в сам маршрут
            nodes.AddRange(productData.GetProducts().Select(p => new SitemapNode(Url.Action(nameof(CatalogController.Details), "Catalog", new { p.Id }))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
