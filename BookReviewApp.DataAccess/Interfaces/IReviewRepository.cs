using BookReviewApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.DataAccess.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);
        Task<Review?> GetUserReviewForBookAsync(Guid bookId, string userId);
        Task SaveChangesAsync();
        Task<List<UserReviewDto>> GetAllUserReviewsAsync(string userId);
    }
}
