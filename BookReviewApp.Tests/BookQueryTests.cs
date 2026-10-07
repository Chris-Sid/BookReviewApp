using BookReviewApp.Entities.Models;
using Xunit;

namespace BookReviewApp.Tests
{
    public class BookQueryTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(-5, 1)]
        [InlineData(3, 3)]
        public void Page_IsClampedToMinimumOfOne(int input, int expected)
        {
            var query = new BookQuery { Page = input };
            Assert.Equal(expected, query.Page);
        }

        [Theory]
        [InlineData(0, BookQuery.DefaultPageSize)]
        [InlineData(1000, BookQuery.MaxPageSize)]
        [InlineData(20, 20)]
        public void PageSize_IsClamped(int input, int expected)
        {
            var query = new BookQuery { PageSize = input };
            Assert.Equal(expected, query.PageSize);
        }

        [Fact]
        public void PagedResult_ComputesTotalPages()
        {
            var result = new PagedResult<Book> { Page = 1, PageSize = 10, TotalCount = 21 };
            Assert.Equal(3, result.TotalPages);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);
        }
    }
}