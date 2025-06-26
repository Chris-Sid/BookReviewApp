using BookReviewApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(string? genre = null, int? year = null);
        Task<IEnumerable<Book>> GetAllWithReviewsAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
    }
}
