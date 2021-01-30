using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Controllers;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [TestInitialize]
        public void Init()
        {
            _controller = new HomeController();
        }

        [TestMethod]
        public void Index_returns_view()
        {
            var result = _controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Throw_throws_exception()
        {
            const string expectedMessage = "some_specific_text";

            var exeption = Assert.ThrowsException<ApplicationException>(() => _controller.Throw(expectedMessage));

            Assert.AreEqual(expectedMessage, exeption.Message);
        }

        [TestMethod]
        public void SecondAction_returns_content()
        {
            const string expectedContent = "Second controller action";

            var result = _controller.SecondAction();

            Assert.IsInstanceOfType(result, typeof(ContentResult));
            Assert.AreEqual(expectedContent, ((ContentResult)result).Content);
        }

        [TestMethod]
        public void Blogs_returns_view()
        {
            var result = _controller.Blogs();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void BlogSingle_returns_view()
        {
            var result = _controller.BlogSingle();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ContactUs_returns_view()
        {
            var result = _controller.ContactUs();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Error404_returns_view()
        {
            var result = _controller.Error404();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ErrorStatus_404_redirects_to_Error404()
        {
            var result = _controller.ErrorStatus("404");

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirect = (RedirectToActionResult)result;

            //Переадресовали на метод Error404
            Assert.AreEqual(nameof(HomeController.Error404), redirect.ActionName);

            //Переадресация внутри того же контроллера
            Assert.IsNull(redirect.ControllerName);
        }
    }
}
