using BookReviewApp.Business.Interfaces;
using BookReviewApp.Entities.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookReviewApp.Business.Services
{
    public class CachedBookService(IDistributedCache cache, BookService inner) : IBookService
    {
        private const string VersionKey = "books:version";

        private static readonly JsonSerializerOptions Json = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        private static readonly DistributedCacheEntryOptions Options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        // Reads: cache-aside
        public async Task<IEnumerable<Book>> GetFilteredBooksAsync(string? genre, int? year, int? rating)
        {
            var key = await KeyAsync($"filtered:{genre}:{year}:{rating}");
            return (await GetOrSetAsync(key, async () =>
                (await inner.GetFilteredBooksAsync(genre, year, rating)).ToList()))!;
        }

        public async Task<Book?> GetBookAsync(Guid id)
        {
            var key = await KeyAsync($"id:{id}");
            return await GetOrSetAsync(key, () => inner.GetBookAsync(id));
        }

        public async Task<PagedResult<Book>> GetPagedBooksAsync(BookQuery query, CancellationToken ct = default)
        {
            var key = await KeyAsync($"paged:{JsonSerializer.Serialize(query, Json)}");
            return (await GetOrSetAsync(key, () => inner.GetPagedBooksAsync(query, ct)))!;
        }

        // Writes: delegate, then invalidate
        public async Task AddBookAsync(Book book)
        {
            await inner.AddBookAsync(book);
            await InvalidateAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            await inner.UpdateBookAsync(book);
            await InvalidateAsync();
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await inner.DeleteBookAsync(id);
            await InvalidateAsync();
        }

        // Helpers
        private async Task<string> KeyAsync(string suffix)
        {
            var version = await cache.GetStringAsync(VersionKey) ?? "0";
            return $"books:{version}:{suffix}";
        }

        // Changing the version makes every old key unreachable. They expire by TTL,
        // so we never need to scan or delete keys in Redis.
        public Task InvalidateAsync() =>
            cache.SetStringAsync(VersionKey, Guid.NewGuid().ToString("N"));

        private async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory)
        {
            var cached = await cache.GetStringAsync(key);
            if (cached is not null)
                return JsonSerializer.Deserialize<T>(cached, Json);

            var value = await factory();
            if (value is not null)
                await cache.SetStringAsync(key, JsonSerializer.Serialize(value, Json), Options);
            return value;
        }
    }
}
