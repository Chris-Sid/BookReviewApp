using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Contracts.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public int Rating { get; set; }
        public string Content { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}
