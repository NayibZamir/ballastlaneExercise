using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crud.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Moq;
using Crud.Data.Model;
using Crud.Data;

namespace Crud.Business.Managers.Tests
{
    [TestClass()]
    public class BookManagerTests
    {
        private Mock<IBookRepository> _mockBookRepository = new Mock<IBookRepository>();
        [TestMethod("Should return a book object")]
        public async Task CreateBookTest()
        {
            var idBook = 1;
            var input = new Book
            {
                Title = "Iliada",
                Author = "Homero",
                Description = "La iliada",
                ReleaseDate = new DateTime(2000, 1, 1)
            };

            _mockBookRepository
                .Setup(manager => manager
                    .CreateBook(It.IsAny<BookDto>()))
                .ReturnsAsync(new BookDto
                {
                    IdBook = idBook,
                    Title = "Iliada",
                    Author = "Homero",
                    Description = "La iliada",
                    ReleaseDate = new DateTime(2000, 1, 1)
                });
            var bookManager = new BookManager(_mockBookRepository.Object);
            var result = await bookManager.CreateBook(input);
            input.IdBook = idBook;
            Assert.AreEqual(idBook, result.IdBook);
            Assert.AreEqual(result.Title, input.Title);
            //await CreateBookTest();
        }

        [TestMethod("Should return an excpetion")]
        public async Task CreateBookTestShoudThrowException()
        {
            _mockBookRepository
                .Setup(manager => manager
                    .CreateBook(It.IsAny<BookDto>()))
                .ThrowsAsync(new Exception());
            var bookManager = new BookManager(_mockBookRepository.Object);
            await Assert.ThrowsExceptionAsync<Exception>(() => bookManager.CreateBook(new Book()));
        }
    }
}