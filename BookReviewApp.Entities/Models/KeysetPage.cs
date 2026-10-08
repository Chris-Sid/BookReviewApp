namespace BookReviewApp.Entities.Models
{
    public enum PageDirection { Next = 0, Previous = 1 }

    public sealed class BookCursorQuery
    {
        public const int MaxPageSize = 100;
        public const int DefaultPageSize = 5;

        private int _pageSize = DefaultPageSize;

        public string? Genre { get; set; }
        public int? Year { get; set; }

        /// <summary>Opaque cursor returned by a previous response.</summary>
        public string? Cursor { get; set; }
        public PageDirection Direction { get; set; } = PageDirection.Next;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(value, MaxPageSize);
        }
    }

    public sealed class KeysetPage<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
        public int PageSize { get; init; }
        public bool HasNext { get; init; }
        public bool HasPrevious { get; init; }
        public string? NextCursor { get; init; }
        public string? PreviousCursor { get; init; }
    }
}   