using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPIControllerTests
    {
        [TestMethod]
        public async Task Index_returns_view_with_values()
        {
            //Arrange
            var expectedValues = new[] { "100", "150", "113", "150" };

            var valueServiceMock = new Mock<IValuesService>();
            valueServiceMock.Setup(s => s.GetAsync()).ReturnsAsync(expectedValues);

            var controller = new WebAPIController(valueServiceMock.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = AssertExt.IsType<ViewResult>(result);
            var model = AssertExt.IsType<IEnumerable<string>>(viewResult.Model);
            CollectionAssert.AreEqual(expectedValues, model.ToArray());

            valueServiceMock.Verify(s => s.GetAsync(), Times.Once);
            valueServiceMock.VerifyNoOtherCalls();
        }

    }
}
