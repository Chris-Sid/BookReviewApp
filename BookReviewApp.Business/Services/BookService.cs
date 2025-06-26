using BookReviewApp.Business.Interfaces;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.DataAccess.Repositories;
using BookReviewApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository repo) => _repo = repo;

        public async Task<IEnumerable<Book>> GetFilteredBooksAsync(string? genre, int? year, int? rating)
        {
            var books = await _repo.GetAllWithReviewsAsync();

            if (!string.IsNullOrWhiteSpace(genre))
                books = books.Where(b => b.Genre == genre);

            if (year.HasValue)
                books = books.Where(b => b.PublishedYear == year.Value);

            if (rating.HasValue)
                books = books.Where(b => b.Reviews.Any())
                             .Where(b => b.Reviews.Average(r => r.Rating) >= rating.Value);

            return books;
        }


        public Task<Book?> GetBookAsync(Guid id) => _repo.GetByIdAsync(id);

        public Task AddBookAsync(Book book) => _repo.AddAsync(book);

        public Task UpdateBookAsync(Book book) => _repo.UpdateAsync(book);
        public Task DeleteBookAsync(Guid Id) => _repo.DeleteAsync(Id);

    }
}
