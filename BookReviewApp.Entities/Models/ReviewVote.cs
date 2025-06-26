using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Entities.Models
{
    public class ReviewVote
    {
        public Guid Id { get; set; }

        public Guid ReviewId { get; set; }
        public Review Review { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public bool IsUpvote { get; set; }
    }
}
