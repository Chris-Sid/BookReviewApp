using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.DataAccess.Repositories;
using BookReviewApp.DataAccess.Services;
using BookReviewApp.Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookReviewApp.Tests
{
    public class ReviewServiceTests
    {
        private readonly ReviewService _service;
        private readonly Mock<IReviewRepository> _repoMock;

        public ReviewServiceTests()
        {
            _repoMock = new Mock<IReviewRepository>();
            _service = new ReviewService(_repoMock.Object);
        }

        [Fact]
        public async Task GetReviewByUserAsync_ReturnsReview_WhenExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var userId = "user123";
            var expectedReview = new Review
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                UserId = userId,
                Content = "Test review",
                Rating = 5
            };

            _repoMock.Setup(r => r.GetUserReviewForBookAsync(bookId, userId))
                     .ReturnsAsync(expectedReview);

            // Act
            var result = await _service.GetReviewByUserAsync(bookId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReview.Content, result!.Content);
        }

 
    }
}
