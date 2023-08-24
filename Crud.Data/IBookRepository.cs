using Crud.Data.Model;

namespace Crud.Data
{
    public interface IBookRepository
    {
        Task<List<BookDto>> GetBooks();
        Task<BookDto> GetBook(int id);
        Task<BookDto> CreateBook(BookDto book);
        Task UpdateBook(BookDto book);
        Task DeleteBook(int id);
    }
}
