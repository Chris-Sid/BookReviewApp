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
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context) => _context = context;

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();


        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
        public async Task<Review?> GetUserReviewForBookAsync(Guid bookId, string userId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);
        }

        public async Task<List<UserReviewDto>> GetAllUserReviewsAsync(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)
                .Include(r => r.Votes)
                .Select(r => new UserReviewDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title,
                    Rating = r.Rating,
                    Content = r.Content,
                    DateCreated = r.DateCreated,
                    Likes = r.Votes.Count(v => v.IsUpvote),
                    Dislikes = r.Votes.Count(v => !v.IsUpvote)
                })
                .ToListAsync();
        }
    }
}
