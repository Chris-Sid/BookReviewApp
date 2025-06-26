using BookReviewApp.Business.Interfaces;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace BookReviewApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? genre, int? year, int? rating)
        {
            var allBooks = await _bookService.GetFilteredBooksAsync(genre, year, rating);
            ViewBag.Genres = allBooks.Select(b => b.Genre).Distinct().OrderBy(g => g).ToList();
            ViewBag.Years = allBooks.Select(b => b.PublishedYear).Distinct().OrderByDescending(y => y).ToList();
            ViewBag.Ratings = Enumerable.Range(1, 5).ToList();

            return View(allBooks);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _bookService.GetBookAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Book model)
        {
            if (!ModelState.IsValid) return View(model);
            await _bookService.AddBookAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _bookService.GetBookAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (!ModelState.IsValid) return View(book);
            await _bookService.UpdateBookAsync(book);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteBookAsync(id);
            return RedirectToAction("Index");
        }

    }

}
