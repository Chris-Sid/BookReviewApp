using BookReviewApp.Business.Services;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookReviewApp.Tests
{
    public class CachedBookServiceTests
    {
        private readonly Mock<IBookRepository> _repo = new();
        private readonly CachedBookService _sut;

        public CachedBookServiceTests()
        {
            IDistributedCache cache =
                new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            _sut = new CachedBookService(cache, new BookService(_repo.Object));

            _repo.Setup(r => r.GetAllWithReviewsAsync())
                 .ReturnsAsync(new List<Book> { new() { Id = Guid.NewGuid(), Genre = "Fantasy" } });
        }

        [Fact]
        public async Task GetFilteredBooks_SecondCall_ComesFromCache()
        {
            await _sut.GetFilteredBooksAsync("Fantasy", null, null);
            await _sut.GetFilteredBooksAsync("Fantasy", null, null);

            _repo.Verify(r => r.GetAllWithReviewsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetFilteredBooks_DifferentFilters_UseDifferentCacheEntries()
        {
            await _sut.GetFilteredBooksAsync("Fantasy", null, null);
            await _sut.GetFilteredBooksAsync("Horror", null, null);

            _repo.Verify(r => r.GetAllWithReviewsAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task AddBook_InvalidatesCache()
        {
            await _sut.GetFilteredBooksAsync(null, null, null);   // populates the cache
            await _sut.AddBookAsync(new Book { Id = Guid.NewGuid() });
            await _sut.GetFilteredBooksAsync(null, null, null);   // must hit the repo again

            _repo.Verify(r => r.GetAllWithReviewsAsync(), Times.Exactly(2));
            _repo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        }
    }
}
