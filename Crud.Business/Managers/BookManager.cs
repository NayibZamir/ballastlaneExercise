using Crud.Data;
using Crud.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Business.Managers
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;

        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> CreateBook(Book book)
        {
            try
            {
                var bookDto = new BookDto
                {
                    IdBook = book.IdBook,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    ReleaseDate = book.ReleaseDate,
                };
                var result = await _bookRepository.CreateBook(bookDto);
                return new Book
                {
                    IdBook = result.IdBook,
                    Title = result.Title,
                    Author = result.Author,
                    Description = result.Description,
                    ReleaseDate = result.ReleaseDate,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBook(int id)
        {
            try
            {
                await _bookRepository.DeleteBook(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book> GetBook(int id)
        {
            try
            {
                var result = await _bookRepository.GetBook(id);

                return (result == null)
                    ? null
                    : new Book
                    {
                        IdBook = result.IdBook,
                        Title = result.Title,
                        Author = result.Author,
                        Description = result.Description,
                        ReleaseDate = result.ReleaseDate,
                    };

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Book>> GetBooks()
        {
            try
            {
                return (await _bookRepository.GetBooks())
                    .ConvertAll(result => new Book
                    {
                        IdBook = result.IdBook,
                        Title = result.Title,
                        Author = result.Author,
                        Description = result.Description,
                        ReleaseDate = result.ReleaseDate,
                    });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateBook(Book book)
        {
            try
            {
                var bookDto = new BookDto
                {
                    IdBook = book.IdBook,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    ReleaseDate = book.ReleaseDate,

                };
                await _bookRepository.UpdateBook(bookDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
