using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Business.Managers
{
    public interface IBookManager
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(int id);
        Task<Book> CreateBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int id);
    }
}
