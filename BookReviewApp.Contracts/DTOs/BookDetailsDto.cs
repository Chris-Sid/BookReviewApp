using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Contracts.DTOs
{
    public class BookDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int PublishedYear { get; set; }
        public string Genre { get; set; } = null!;
        public double AverageRating { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
