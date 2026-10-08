using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.DataAccess.Pagination;
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
        private readonly IBookCursorCodec _cursorCodec;

        public BookRepository(AppDbContext context, IBookCursorCodec cursorCodec)
        {
            _context = context;
            _cursorCodec = cursorCodec;
        }

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

        public async Task<KeysetPage<Book>> GetKeysetPageAsync(BookCursorQuery query, CancellationToken cancellationToken = default)
        {
            var cursor = _cursorCodec.Decode(query.Cursor);

            // A backward request without a cursor is meaningless; treat it as the first page.
            var forward = cursor is null || query.Direction == PageDirection.Next;

            var books = ApplyFilters(_context.Books.AsNoTracking(), query.Genre, query.Year);

            if (cursor is not null)
            {
                var (title, id) = cursor;

                // Seek predicate: translates to an index range scan, not a row skip.
                books = forward
                    ? books.Where(b => string.Compare(b.Title, title) > 0 ||
                                       (b.Title == title && b.Id.CompareTo(id) > 0))
                    : books.Where(b => string.Compare(b.Title, title) < 0 ||
                                       (b.Title == title && b.Id.CompareTo(id) < 0));
            }

            var ordered = forward
                ? books.OrderBy(b => b.Title).ThenBy(b => b.Id)
                : books.OrderByDescending(b => b.Title).ThenByDescending(b => b.Id);

            // Fetch one extra row to learn whether another page exists, with no COUNT(*).
            var rows = await ordered.Take(query.PageSize + 1).ToListAsync(cancellationToken);

            var hasMore = rows.Count > query.PageSize;
            if (hasMore) rows.RemoveAt(query.PageSize);
            if (!forward) rows.Reverse();

            if (rows.Count == 0)
            {
                return new KeysetPage<Book> { PageSize = query.PageSize };
            }

            // Moving forward: more rows ahead = hasMore, and we came from somewhere if a cursor was supplied.
            // Moving backward: the mirror image.
            var hasNext = forward ? hasMore : true;
            var hasPrevious = forward ? cursor is not null : hasMore;

            return new KeysetPage<Book>
            {
                Items = rows,
                PageSize = query.PageSize,
                HasNext = hasNext,
                HasPrevious = hasPrevious,
                NextCursor = hasNext ? _cursorCodec.Encode(ToCursor(rows[^1])) : null,
                PreviousCursor = hasPrevious ? _cursorCodec.Encode(ToCursor(rows[0])) : null
            };
        }

        private static IQueryable<Book> ApplyFilters(IQueryable<Book> books, string? genre, int? year)
        {
            if (!string.IsNullOrWhiteSpace(genre))
                books = books.Where(b => b.Genre == genre);

            if (year.HasValue)
                books = books.Where(b => b.PublishedYear == year);

            return books;
        }

        private static BookCursor ToCursor(Book book) => new(book.Title, book.Id);
    }
}
