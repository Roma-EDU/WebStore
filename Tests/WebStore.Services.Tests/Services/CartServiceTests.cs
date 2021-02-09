// Ω
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WebStore.Domain;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using Xunit;

namespace WebStore.Services.Tests.Services
{
    /* xUnit tests
     * [TestMethod] = [Fact]
     * [TestInitialize] = constructor
     * [TestCleanup] = IDisposable.Dispose
     * [TestClass] - не используется, поиск идёт за счёт [Fact] 
     */
    public class CartServiceTests
    {
        private readonly Cart _cart;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _cart = new Cart();

            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(_cart);

            var productDataMock = new Mock<IProductData>();

            _cartService = new CartService(productDataMock.Object, cartStoreMock.Object);
        }


        [Fact]
        public void AddToCart_add_new_item_to_empty_cart_works()
        {
            //Arrange
            const int newProductId = 12;

            //Act
            _cartService.AddToCart(newProductId);

            //Assert
            Assert.Equal(1, _cart.Items.Count);

            var cartItem = _cart.Items.First();

            Assert.Equal(newProductId, cartItem.ProductId);
            Assert.Equal(1, cartItem.Quantity);
        }

        [Fact]
        public void AddToCart_add_new_item_to_filled_cart_works()
        {
            //Arrange
            const int newProductId = 13;

            var existingCartItem = new CartItem() { ProductId = 5, Quantity = 2 };
            _cart.Items.Add(existingCartItem);

            //Act
            _cartService.AddToCart(newProductId);

            //Assert
            Assert.Equal(2, _cart.Items.Count);

            var cartItem = _cart.Items.FirstOrDefault(c => c.ProductId == newProductId);
            Assert.NotNull(cartItem);
            Assert.Equal(1, cartItem.Quantity);
        }

        [Fact]
        public void AddToCart_add_existing_item_increments_quantity()
        {
            //Arrange
            const int existingProductId = 23;
            const int existingQuantity = 7;

            var existingCartItem = new CartItem() { ProductId = existingProductId, Quantity = existingQuantity };
            _cart.Items.Add(existingCartItem);

            //Act
            _cartService.AddToCart(existingProductId);

            //Assert
            Assert.Equal(1, _cart.Items.Count);
            Assert.Equal(existingQuantity + 1, _cart.ItemsCount);
        }

        [Fact]
        public void DecrementFromCart_not_existing_item_do_nothing()
        {
            //Arrange
            const int existingProductId = 23;
            const int existingQuantity = 7;

            const int notExistingProductId = 1;

            var existingCartItem = new CartItem() { ProductId = existingProductId, Quantity = existingQuantity };
            _cart.Items.Add(existingCartItem);

            //Act
            _cartService.DecrementFromCart(notExistingProductId);

            //Assert
            Assert.Equal(1, _cart.Items.Count);
            Assert.Equal(existingQuantity, _cart.ItemsCount);
            Assert.Equal(existingProductId, _cart.Items.First().ProductId);
        }

        [Fact]
        public void DecrementFromCart_existing_item_decrements_quantity()
        {
            //Arrange
            const int existingProductId = 28;
            const int existingQuantity = 7;

            var existingCartItem = new CartItem() { ProductId = existingProductId, Quantity = existingQuantity };
            _cart.Items.Add(existingCartItem);

            //Act
            _cartService.DecrementFromCart(existingProductId);

            //Assert
            Assert.Equal(1, _cart.Items.Count);
            Assert.Equal(existingQuantity - 1, _cart.ItemsCount);
            Assert.Equal(existingProductId, _cart.Items.First().ProductId);
        }

        [Fact]
        public void DecrementFromCart_existing_last_item_removes_it()
        {
            //Arrange
            const int existingProductId = 34;
            const int existingSecontProductId = 35;
            const int existingSecontQuantity = 1;

            var existingCartItem = new CartItem() { ProductId = existingProductId, Quantity = 1 };
            var existingSecontCartItem = new CartItem() { ProductId = existingSecontProductId, Quantity = existingSecontQuantity };
            _cart.Items.Add(existingCartItem);
            _cart.Items.Add(existingSecontCartItem);

            //Act
            _cartService.DecrementFromCart(existingProductId);

            //Assert
            Assert.Single(_cart.Items);
            Assert.Equal(existingSecontQuantity, _cart.ItemsCount);

            var cartItem = _cart.Items.First();
            Assert.Equal(existingSecontProductId, cartItem.ProductId);
            Assert.Equal(existingSecontQuantity, cartItem.Quantity);
        }

        [Fact]
        public void RemoveFromCart_existing_item_works()
        {
            //Arrange
            const int existingProductId1 = 1;
            const int existingProductId2 = 34;
            const int existingProductId3 = 78;
            const int quantity1 = 4;
            const int quantity2 = 19;
            const int quantity3 = 99;

            var existingCartItem1 = new CartItem() { ProductId = existingProductId1, Quantity = quantity1 };
            var existingCartItem2 = new CartItem() { ProductId = existingProductId2, Quantity = quantity2 };
            var existingCartItem3 = new CartItem() { ProductId = existingProductId3, Quantity = quantity3 };

            _cart.Items.Add(existingCartItem1);
            _cart.Items.Add(existingCartItem2);
            _cart.Items.Add(existingCartItem3);

            //Act
            _cartService.RemoveFromCart(existingProductId2);

            //Assert
            Assert.Equal(2, _cart.Items.Count);

            var cartItem1 = _cart.Items.FirstOrDefault(i => i.ProductId == existingProductId1);
            Assert.NotNull(cartItem1);
            Assert.Equal(quantity1, cartItem1.Quantity);

            var cartItem2 = _cart.Items.FirstOrDefault(i => i.ProductId == existingProductId2);
            Assert.Null(cartItem2);

            var cartItem3 = _cart.Items.FirstOrDefault(i => i.ProductId == existingProductId3);
            Assert.NotNull(cartItem3);
            Assert.Equal(quantity3, cartItem3.Quantity);
        }

        [Fact]
        public void RemoveFromCart_not_existing_item_do_nothing()
        {
            //Arrange
            const int existingProductId1 = 1;
            const int existingProductId2 = 34;
            const int notExistingProductId = 78;
            const int quantity1 = 4;
            const int quantity2 = 19;

            var existingCartItem1 = new CartItem() { ProductId = existingProductId1, Quantity = quantity1 };
            var existingCartItem2 = new CartItem() { ProductId = existingProductId2, Quantity = quantity2 };

            _cart.Items.Add(existingCartItem1);
            _cart.Items.Add(existingCartItem2);

            //Act
            _cartService.RemoveFromCart(notExistingProductId);

            //Assert
            Assert.Equal(2, _cart.Items.Count);

            var cartItem1 = _cart.Items.FirstOrDefault(i => i.ProductId == existingProductId1);
            Assert.NotNull(cartItem1);
            Assert.Equal(quantity1, cartItem1.Quantity);

            var cartItem2 = _cart.Items.FirstOrDefault(i => i.ProductId == existingProductId2);
            Assert.NotNull(cartItem2);
            Assert.Equal(quantity2, cartItem2.Quantity);
        }

        [Fact]
        public void Clear_empty_cart_do_nothing()
        {
            _cartService.Clear();

            Assert.Empty(_cart.Items);
        }

        [Fact]
        public void Clear_removes_all_items()
        {
            _cart.Items.Add(new CartItem() { ProductId = 3, Quantity = 1 });
            _cart.Items.Add(new CartItem() { ProductId = 908, Quantity = 513 });
            _cart.Items.Add(new CartItem() { ProductId = 11, Quantity = 5 });

            _cartService.Clear();

            Assert.Empty(_cart.Items);
        }

        [Fact]
        public void TransformCart_works()
        {
            //Arrange
            const int productId1 = 1;
            const int productId2 = 2;

            const int quantity1 = 2;
            const int quantity2 = 3;

            const decimal price1 = 7.7m;
            const decimal price2 = 11.11m;

            const int totalQuantity = quantity1 + quantity2;
            const decimal totalPrice = quantity1 * price1 + quantity2 * price2;

            var cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = productId1, Quantity = quantity1 },
                    new CartItem() { ProductId = productId2, Quantity = quantity2 },
                }
            };
            
            var products = new List<ProductDto>()
            {
                createDto(productId1, price1),
                createDto(productId2, price2),
            };
            var pagedProducts = new PagedProductDto(products, products.Count);

            var productData = new Mock<IProductData>();
            productData.Setup(c => c.GetProducts(It.IsAny<ProductFilter>())).Returns(pagedProducts);
            
            var cartStore = new Mock<ICartStore>();
            cartStore.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productData.Object, cartStore.Object);

            //Act
            var result = cartService.TransformFromCart();

            //Assert
            Assert.Equal(totalQuantity, result.ItemsCount);
            Assert.Equal(totalPrice, result.TotalPrice);
        }

        private ProductDto createDto(int productId, decimal price)
        {
            int brandId = productId;
            int sectionId = productId;
            return new ProductDto(
                Id: productId,
                Name: $"Test name {productId}",
                Order: productId - 1,
                Price: price,
                ImageUrl: $"Test image url {productId}.jpg",
                Brand: new BrandDto(brandId, $"Test brand {brandId}", brandId - 1, brandId * 3),
                Section: new SectionDto(sectionId, $"Test section {sectionId}", sectionId - 1, null, sectionId * 5)
            );
        }
    }
}
