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
    public class ReviewVoteRepository : IReviewVoteRepository
    {
        private readonly AppDbContext _context;

        public ReviewVoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewVote?> GetUserVoteAsync(Guid reviewId, string userId)
        {
            return await _context.ReviewVotes
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId);
        }

        public async Task AddVoteAsync(ReviewVote vote)
        {
            await _context.ReviewVotes.AddAsync(vote);
        }

        public Task UpdateVoteAsync(ReviewVote vote)
        {
            _context.ReviewVotes.Update(vote);
            return Task.CompletedTask;
        }


        public async Task<bool> HasUserVotedAsync(Guid reviewId, string userId)
        {
            return await _context.ReviewVotes
                .AnyAsync(v => v.ReviewId == reviewId && v.UserId == userId);
        }

        public async Task RemoveVoteAsync(ReviewVote vote)
        {
            _context.ReviewVotes.Remove(vote);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
