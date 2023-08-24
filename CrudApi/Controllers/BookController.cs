using Crud.Business;
using Crud.Business.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookManager _bookManager;
        public BookController(ILogger<BookController> logger, IBookManager bookManager)
        {
            _logger = logger;
            _bookManager = bookManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Book>> GetAsync()
        {
            return await _bookManager.GetBooks();
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var book = await _bookManager.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<Book> CreateBook(Book book)
        {
            return await _bookManager.CreateBook(book);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBook(Book book)
        {
            if (await _bookManager.GetBook(book.IdBook) == null)
            {
                return NotFound();
            }
            await _bookManager.UpdateBook(book);
            return Ok();
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            if (await _bookManager.GetBook(id) == null)
            {
                return NotFound();
            }
            await _bookManager.DeleteBook(id);
            return Ok();
        }
    }
}
