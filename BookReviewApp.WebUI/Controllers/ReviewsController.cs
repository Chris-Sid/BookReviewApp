using BookReviewApp.Business.Interfaces;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApp.WebUI.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewVoteRepository _voteRepository;
        private readonly UserManager<AppUser> _userManager;
        public ReviewsController(IReviewRepository reviewRepository, IReviewVoteRepository voteRepository, UserManager<AppUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _voteRepository = voteRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> AddReview(Guid bookId, int rating, string content)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var existingReview = await _reviewRepository.GetUserReviewForBookAsync(bookId, userId);

            if (existingReview != null)
            {
                return RedirectToAction("EditReview", new { id = existingReview.Id });
            }

            var review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = rating,
                Content = content,
                DateCreated = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [Authorize(Roles = "Customer,Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateReview(Review model)
        {
            var userId = _userManager.GetUserId(User);

            var existing = await _reviewRepository.GetUserReviewForBookAsync(model.BookId, userId);
            if (existing == null || existing.UserId != userId)
                return Forbid();

            existing.Content = model.Content;
            existing.Rating = model.Rating;
            await _reviewRepository.SaveChangesAsync();

            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Vote(Guid BookID, Guid reviewId, bool isUpvote)
        {
            var userId = _userManager.GetUserId(User);
            var existingVote = await _voteRepository.GetUserVoteAsync(reviewId, userId);

            if (existingVote == null)
            {
                // First time voting
                var vote = new ReviewVote
                {
                    ReviewId = reviewId,
                    UserId = userId,
                    IsUpvote = isUpvote
                };

                await _voteRepository.AddVoteAsync(vote);
            }
            else if (existingVote.IsUpvote == isUpvote)
            {
                // Same vote clicked again: remove it (toggle off)
                await _voteRepository.RemoveVoteAsync(existingVote);
            }
            else if (existingVote.IsUpvote != isUpvote)
            {
                // Change vote (from like to dislike or vice versa)
                existingVote.IsUpvote = isUpvote;
                await _voteRepository.UpdateVoteAsync(existingVote);
            }
            else
            {
                // Change vote
                existingVote.IsUpvote = isUpvote;
                await _voteRepository.UpdateVoteAsync(existingVote);
            }

            await _voteRepository.SaveChangesAsync();

            return RedirectToAction("Details", "Books", new { id = BookID });
        }
    }
}
