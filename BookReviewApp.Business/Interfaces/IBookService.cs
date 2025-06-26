using BookReviewApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Business.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetFilteredBooksAsync(string? genre, int? year, int? rating);
        Task<Book?> GetBookAsync(Guid id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Guid Id);
    }
}
