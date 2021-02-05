using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Services.Data;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Throw(string id) => throw new ApplicationException(id);

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }

        public IActionResult Blogs() => View();
        
        public IActionResult BlogSingle() => View();
        
        public IActionResult ContactUs() => View();

        public IActionResult Error404() => View();

        public IActionResult ErrorStatus(string statusCode)
        {
            switch (statusCode)
            {
                case "404":
                    return RedirectToAction(nameof(Error404));
                default:
                    return Content($"Ошибка {statusCode}");
            }
        }
    }
}
