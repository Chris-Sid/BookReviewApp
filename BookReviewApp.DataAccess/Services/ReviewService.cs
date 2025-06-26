using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.DataAccess.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review?> GetReviewByUserAsync(Guid bookId, string userId)
        {
            return await _reviewRepository.GetUserReviewForBookAsync(bookId, userId);
        }
    }
}
