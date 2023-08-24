using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrudApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crud.Business.Managers;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Controllers.Tests
{
    [TestClass()]
    public class BookControllerTests
    {
        private Mock<IBookManager> _bookManager = new();
        private Mock<ILogger<BookController>> _logger = new();

        [TestMethod("Should return Ok")]
        public async Task GetByIdAsyncTest()
        {
            _bookManager.Setup(manager => manager
                        .GetBook(It.IsAny<int>()))
                .ReturnsAsync(new Crud.Business.Book());
            var bookController = new BookController(_logger.Object, _bookManager.Object);
            var result = await bookController.GetByIdAsync(It.IsAny<int>());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod("Should return Not Found(404)")]
        public async Task GetByIdAsyncTestNotFound()
        {
            _bookManager.Setup(manager => manager
                        .GetBook(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            var bookController = new BookController(_logger.Object, _bookManager.Object);
            var result = await bookController.GetByIdAsync(It.IsAny<int>());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod("Should rise an exception")]
        public async Task GetByIdAsyncTestException()
        {
            _bookManager.Setup(manager => manager
                        .GetBook(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            var bookController = new BookController(_logger.Object, _bookManager.Object);

            await Assert.ThrowsExceptionAsync<Exception>(() => bookController.GetByIdAsync(It.IsAny<int>()));
        }
    }
}