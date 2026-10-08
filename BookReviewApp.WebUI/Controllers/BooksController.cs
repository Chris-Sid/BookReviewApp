using BookReviewApp.Business.Interfaces;
using BookReviewApp.DataAccess.Pagination;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        //[AllowAnonymous]
        //public async Task<IActionResult> Index([FromQuery] BookQuery query, CancellationToken cancellationToken)
        //{
        //    var result = await _bookService.GetPagedBooksAsync(query, cancellationToken);

        //    // Out-of-range page (e.g. after deleting items): go to the last valid page
        //    if (result.TotalPages > 0 && query.Page > result.TotalPages)
        //    {
        //        query.Page = result.TotalPages;
        //        return RedirectToAction(nameof(Index), new { query.Genre, query.Year, query.Page, query.PageSize });
        //    }

        //    ViewBag.Query = query;
        //    return View(result);
        //}

        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] BookCursorQuery query, CancellationToken cancellationToken)
        {
            // Default page size if not set
            if (query.PageSize <= 0) query.PageSize = BookQuery.DefaultPageSize;

            try
            {
                var result = await _bookService.GetKeysetPageAsync(query, cancellationToken);

                ViewBag.Query = query;
                return View(result);
            }
            catch (InvalidCursorException)
            {
                // Tampered or stale cursor (e.g. data-protection keys rotated): restart from page one.
                return RedirectToAction(nameof(Index), new { query.Genre, query.Year, query.PageSize });
            }
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
