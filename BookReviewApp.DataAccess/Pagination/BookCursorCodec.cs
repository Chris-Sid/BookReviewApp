using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace BookReviewApp.DataAccess.Pagination
{
    public sealed record BookCursor(string Title, Guid Id);

    public interface IBookCursorCodec
    {
        string Encode(BookCursor cursor);
        BookCursor? Decode(string? token);
    }

    public sealed class BookCursorCodec : IBookCursorCodec
    {
        private readonly IDataProtector _protector;

        public BookCursorCodec(IDataProtectionProvider provider)
            => _protector = provider.CreateProtector("BookReviewApp.BookCursor.v1");

        public string Encode(BookCursor cursor)
        {
            var payload = JsonSerializer.SerializeToUtf8Bytes(cursor);
            return Base64Url.EncodeToString(_protector.Protect(payload));
        }   

        public BookCursor? Decode(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            try
            {
                var payload = _protector.Unprotect(Base64Url.DecodeFromChars(token));
                return JsonSerializer.Deserialize<BookCursor>(payload);
            }
            catch (Exception ex) when (ex is FormatException or CryptographicException or JsonException)
            {
                throw new InvalidCursorException("The supplied cursor is invalid.", ex);
            }
        }
    }

    public sealed class InvalidCursorException(string message, Exception? inner = null)
        : Exception(message, inner);
}