using BookReviewApp.Business.Interfaces;
using BookReviewApp.Contracts.DTOs;
using BookReviewApp.Contracts.Models;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BookReviewApp.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) => _bookService = bookService;
        /// <summary>
        /// Returns a list of books with optional filters.
        /// </summary>
        /// <param name="genre">Genre filter</param>
        /// <param name="year">Published year filter</param>
        /// <param name="rating">Minimum rating filter</param>
        /// <returns>List of books</returns>
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] BookFilterDto filter)
        {
            var books = await _bookService.GetFilteredBooksAsync(filter.Genre, filter.Year, filter.MinRating);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await _bookService.GetBookAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Genre = dto.Genre,
                PublishedYear = dto.PublishedYear
            };

            await _bookService.AddBookAsync(book);
            return Ok();
        }
    }

}
