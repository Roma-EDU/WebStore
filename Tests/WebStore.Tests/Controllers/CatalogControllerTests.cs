using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.DTOs.Products;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        private ProductDto createDto(int productId, int brandId, int sectionId)
        {
            return new ProductDto(
                Id: productId,
                Name: $"Test name {productId}",
                Order: productId - 1,
                Price: productId * 10,
                ImageUrl: $"Test image url {productId}.jpg",
                Brand: new BrandDto(brandId, $"Test brand {brandId}", brandId - 1, brandId * 3),
                Section: new SectionDto(sectionId, $"Test section {sectionId}", sectionId - 1, null, sectionId * 5)
            );
        }

        [DataTestMethod]
        [DataRow(5, 7)]
        [DataRow(3, null)]
        [DataRow(null, 4)]
        [DataRow(null, null)]
        public void Shop_returns_view_for_brand_and_section(int? brandId, int? sectionId)
        {
            //Arrange
            var products = new[]
            {
                createDto(1, 1, 1),
                createDto(2, 2, 2),
                createDto(3, 3, 3),
            };

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(products);

            var controller = new CatalogController(productDataMock.Object);

            //Act
            var result = controller.Shop(brandId, sectionId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            productDataMock.Verify(p => p.GetProducts(It.IsAny<ProductFilter>()), Times.Once);

            var model = ((ViewResult)result).Model;
            Assert.IsInstanceOfType(model, typeof(CatalogViewModel));

            var catalogVM = (CatalogViewModel)model;
            Assert.AreEqual(brandId, catalogVM.BrandId);
            Assert.AreEqual(sectionId, catalogVM.SectionId);
            Assert.AreEqual(products.Length, catalogVM.Products.Count());
        }

        [TestMethod]
        public void Shop2_returns_view_for_filter()
        {
            //Arrange
            var products = new[]
            {
                createDto(1, 1, 1),
                createDto(2, 2, 2),
            };

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(products);

            var controller = new CatalogController(productDataMock.Object);
            var filter = new ProductFilter() { Ids = new[] { 3, 6, 12 } };

            //Act
            var result = controller.Shop2(filter);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            productDataMock.Verify(p => p.GetProducts(It.IsAny<ProductFilter>()), Times.Once);

            var model = ((ViewResult)result).Model;
            Assert.IsInstanceOfType(model, typeof(CatalogViewModel));

            var catalogVM = (CatalogViewModel)model;
            Assert.AreEqual(filter?.BrandId, catalogVM.BrandId);
            Assert.AreEqual(filter?.SectionId, catalogVM.SectionId);
            Assert.AreEqual(products.Length, catalogVM.Products.Count());
        }

        [TestMethod]
        public void Details_returns_not_found_for_null_product()
        {
            //Arrange
            const int nullProductId = 9;

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<ProductDto>(null);

            var controller = new CatalogController(productDataMock.Object);

            //Act
            var result = controller.Details(nullProductId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            productDataMock.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void Details_returns_view_with_correct_item()
        {
            // Arrange
            const int productId = 4;
            var productDto = createDto(productId, 8, 9);

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns(productDto);

            var controller = new CatalogController(productDataMock.Object);

            // Act
            var result = controller.Details(productId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            productDataMock.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once);

            var model = ((ViewResult)result).Model;
            Assert.IsInstanceOfType(model, typeof(ProductViewModel));

            var productVM = (ProductViewModel)model;
            Assert.AreEqual(productId, productVM.Id);
            Assert.AreEqual(productDto.Name, productVM.Name);
            Assert.AreEqual(productDto.Price, productVM.Price);
            Assert.AreEqual(productDto.Brand.Name, productVM.Brand);
        }
    }
}
