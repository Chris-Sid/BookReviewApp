using BookReviewApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.DataAccess.Interfaces
{
    public interface IReviewVoteRepository
    {
        Task<ReviewVote?> GetUserVoteAsync(Guid reviewId, string userId);
        Task UpdateVoteAsync(ReviewVote vote);
        Task<bool> HasUserVotedAsync(Guid reviewId, string userId);
        Task AddVoteAsync(ReviewVote vote);
        Task RemoveVoteAsync(ReviewVote vote);
        Task SaveChangesAsync();
    }
}
