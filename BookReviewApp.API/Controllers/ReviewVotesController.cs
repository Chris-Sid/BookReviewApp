using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReviewApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewVotesController : ControllerBase
    {
        private readonly IReviewVoteRepository _voteRepository;
        private readonly UserManager<AppUser> _userManager;

        public ReviewVotesController(IReviewVoteRepository voteRepository, UserManager<AppUser> userManager)
        {
            _voteRepository = voteRepository;
            _userManager = userManager;
        }

        [HttpPost("{reviewId}/vote")]
        [Authorize]
        public async Task<IActionResult> Vote(Guid reviewId, [FromQuery] bool isUpvote)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await _voteRepository.HasUserVotedAsync(reviewId, userId))
            {
                return BadRequest("User has already voted on this review.");
            }

            var vote = new ReviewVote
            {
                ReviewId = reviewId,
                UserId = userId,
                IsUpvote = isUpvote
            };

            await _voteRepository.AddVoteAsync(vote);
            await _voteRepository.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }

}
