using Crud.Data;
using Crud.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Crud.Business.Managers.Tests
{
    [TestClass()]
    public class BookManagerTests
    {
        private Mock<IBookRepository> _mockBookRepository = new Mock<IBookRepository>();


        [TestMethod("CreateBook Should return a book object")]
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
                .Setup( repository => repository 
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

        [TestMethod("CreateBook Should return an excpetion")]
        public async Task CreateBookTestShoudThrowException()
        {
            _mockBookRepository
                .Setup( repository => repository 
                    .CreateBook(It.IsAny<BookDto>()))
                .ThrowsAsync(new Exception());
            var bookManager = new BookManager(_mockBookRepository.Object);
            await Assert.ThrowsExceptionAsync<Exception>(() => bookManager.CreateBook(new Book()));
        }

        [TestMethod("DeleteBook shoul run succesfull")]
        public async Task DeleteBookTest()
        {
            _mockBookRepository
                .Setup( repository => repository 
                    .DeleteBook(It.IsAny<int>()));
            var bookManager = new BookManager(_mockBookRepository.Object);
            await bookManager.DeleteBook(1);
        }

        [TestMethod("DeleteBook Should return an excpetion")]
        public async Task DeleteBookTestShoudThrowException()
        {
            _mockBookRepository
                .Setup( repository => repository 
                    .DeleteBook(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            var bookManager = new BookManager(_mockBookRepository.Object);
            await Assert.ThrowsExceptionAsync<Exception>(() => bookManager.DeleteBook(It.IsAny<int>()));
        }

        [TestMethod("GetBook Should return a book object")]
        public async Task GetBookTest()
        {
            var idBook = 1;
            _mockBookRepository
                .Setup( repository => repository 
                    .GetBook(It.IsAny<int>()))
                .ReturnsAsync(new BookDto
                {
                    IdBook = idBook,
                    Title = "Iliada",
                    Author = "Homero",
                    Description = "La iliada",
                    ReleaseDate = new DateTime(2000, 1, 1)
                });
            var bookManager = new BookManager(_mockBookRepository.Object);
            var result = await bookManager.GetBook(idBook);
            Assert.AreEqual(idBook, result.IdBook);
            Assert.IsInstanceOfType(result, typeof(Book));
            //await CreateBookTest();
        }

        [TestMethod("GetBook Should return null")]
        public async Task GetBookTestShouldReturnNull()
        {
            _mockBookRepository
                .Setup( repository => repository 
                    .GetBook(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            var bookManager = new BookManager(_mockBookRepository.Object);
            var result = await bookManager.GetBook(It.IsAny<int>());
            Assert.IsNull(result);
            //await CreateBookTest();
        }

        [TestMethod("GetBook Should return an excpetion")]
        public async Task GetBookTestShoudThrowException()
        {
            _mockBookRepository
                .Setup( repository => repository 
                    .GetBook(It.IsAny<int>()))
                .ThrowsAsync(new Exception());
            var bookManager = new BookManager(_mockBookRepository.Object);
            await Assert.ThrowsExceptionAsync<Exception>(() => bookManager.GetBook(It.IsAny<int>()));
        }
    }
}