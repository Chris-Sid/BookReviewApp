namespace BookReviewApp.Entities.Models
{
    public class BookQuery
    {
        public const int MaxPageSize = 50;
        public const int DefaultPageSize = 10;

        private int _page = 1;
        private int _pageSize = DefaultPageSize;

        public string? Genre { get; set; }
        public int? Year { get; set; }

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? DefaultPageSize : Math.Min(value, MaxPageSize);
        }
    }
}