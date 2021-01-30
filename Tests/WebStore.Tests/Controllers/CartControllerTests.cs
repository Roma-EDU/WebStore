using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTOs.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        private CartController _controller;

        [TestInitialize]
        public void Init()
        {
            var cartServiceMock = new Mock<ICartService>();
            _controller = new CartController(cartServiceMock.Object);
        }

        [TestMethod]
        public void Index_returns_view_with_correct_model()
        {
            var expectedCart = new CartViewModel
            {
                Items = new[] 
                { 
                    (new ProductViewModel { Name = "Test product 1", Price = 10m }, 1),
                    (new ProductViewModel { Name = "Test product 2", Price = 15m }, 3),
                }
            };

            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
               .Setup(c => c.TransformFromCart())
               .Returns(expectedCart);

            var controller = new CartController(cartServiceMock.Object);

            var result = controller.Index();

            var viewResult = AssertExt.IsType<ViewResult>(result);
            var model = AssertExt.IsType<CartOrderViewModel>(viewResult.Model);

            Assert.AreEqual(expectedCart, model.Cart);
        }

        [TestMethod]
        public async Task CheckOut_invalidModelState_returns_view_with_same_model()
        {
            //Arrange
            const string expectedOrderName = "Test order";
            var orderVM = new OrderViewModel { Name = expectedOrderName };

            var orderServiceMock = new Mock<IOrderService>();
            _controller.ModelState.AddModelError("error", "Model is invalid for test");

            //Act
            var result = await _controller.CheckOut(orderVM, orderServiceMock.Object);

            //Assert
            var viewResult = AssertExt.IsType<ViewResult>(result);
            var model = AssertExt.IsType<CartOrderViewModel>(viewResult.Model);

            Assert.AreEqual(expectedOrderName, model.Order.Name);
        }

        [TestMethod]
        public async Task CheckOut_calls_service_and_redirects_to_OrderConfirmed()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
               .Setup(c => c.TransformFromCart())
               .Returns(() => new CartViewModel
               {
                   Items = new[] { (new ProductViewModel { Name = "Test product" }, 1) }
               });

            const int expectedOrderId = 17;
            var orderDto = new OrderDto(
                Id: expectedOrderId,
                Name: "Test order",
                Phone: "+1(234)567-89-00",
                Address: "Test address",
                Date: DateTime.Now,
                Items: Enumerable.Empty<OrderItemDto>()
            );


            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
               .Setup(c => c.CreateOrder(It.IsAny<string>(), It.IsAny<CreateOrderModel>()))
               .ReturnsAsync(orderDto);

            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        //Так задаётся пользователь
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "TestUser") }))
                    }
                }
            };

            var orderVM = new OrderViewModel
            {
                Name = orderDto.Name,
                Address = orderDto.Address,
                Phone = orderDto.Phone,
            };

            var result = await controller.CheckOut(orderVM, orderServiceMock.Object);

            cartServiceMock.Verify(c => c.TransformFromCart(), Times.Once);
            orderServiceMock.Verify(c => c.CreateOrder(It.IsAny<string>(), It.IsAny<CreateOrderModel>()), Times.Once);
            
            var redirect = AssertExt.IsType<RedirectToActionResult>(result);
            Assert.AreEqual(nameof(CartController.OrderConfirmed), redirect.ActionName);
            Assert.IsNull(redirect.ControllerName);

            Assert.AreEqual(expectedOrderId, redirect.RouteValues["id"]);
        }

        [TestMethod]
        public void AddToCart_redirects_to_Index()
        {
            var result = _controller.AddToCart(5);
            AssertRedirectsToIndex(result);
        }

        [TestMethod]
        public void RemoveFromCart_redirects_to_Index()
        {
            var result = _controller.RemoveFromCart(5);
            AssertRedirectsToIndex(result);
        }

        [TestMethod]
        public void DecrementFromCart_redirects_to_Index()
        {
            var result = _controller.DecrementFromCart(5);
            AssertRedirectsToIndex(result);
        }

        [TestMethod]
        public void Clear_redirects_to_Index()
        {
            var result = _controller.Clear();
            AssertRedirectsToIndex(result);
        }

        private void AssertRedirectsToIndex(IActionResult result)
        {
            var redirect = AssertExt.IsType<RedirectToActionResult>(result);

            Assert.AreEqual(nameof(CartController.Index), redirect.ActionName);
            Assert.IsNull(redirect.ControllerName);
        }

        [TestMethod]
        public void OrderConfirmed_returns_view()
        {
            const int orderId = 7;

            var result = _controller.OrderConfirmed(orderId);

            AssertExt.IsType<ViewResult>(result);
        }


        /*
        public IActionResult Index() => View(new CartOrderViewModel
        {
            Cart = _CartService.TransformFromCart()
        });


         */


    }
}
