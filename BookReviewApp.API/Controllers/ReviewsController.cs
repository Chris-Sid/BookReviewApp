using BookReviewApp.Contracts.DTOs;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReviewApp.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewService;
        public ReviewsController(IReviewRepository reviewService) => _reviewService = reviewService;

        // ------------------------------
        // API Documentation (API/Controllers/ReviewsController.cs)
        // ------------------------------
        /// <summary>
        /// Adds a new review to a book.
        /// </summary>
        /// <param name="reviewDto">The review data to add.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var review = new Review
            {
                BookId = dto.BookId,
                Content = dto.Content,
                Rating = dto.Rating,
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };
            await _reviewService.AddAsync(review);
            return Ok();
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetBookReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviews = await _reviewService.GetAllUserReviewsAsync(userId);
            return Ok(reviews);
        }

    }
}
