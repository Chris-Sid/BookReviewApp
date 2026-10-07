using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context) => _context = context;

        public async Task<List<Book>> GetAllAsync(string? genre, int? year)
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(genre)) query = query.Where(b => b.Genre == genre);
            if (year.HasValue) query = query.Where(b => b.PublishedYear == year);
            return await query.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.Include(b => b.Reviews).ThenInclude(r => r.User).Include(b => b.Reviews).ThenInclude(r => r.Votes).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllWithReviewsAsync()
        {
            return await _context.Books
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedResult<Book>> GetPagedAsync(BookQuery query, CancellationToken cancellationToken = default)
        {
            var books = _context.Books.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Genre))
                books = books.Where(b => b.Genre == query.Genre);

            if (query.Year.HasValue)
                books = books.Where(b => b.PublishedYear == query.Year);

            var totalCount = await books.CountAsync(cancellationToken);

            var items = await books
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Id) // tie-breaker keeps paging stable
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Book>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
