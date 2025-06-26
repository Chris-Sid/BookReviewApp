using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Contracts.DTOs
{
    public class UserReviewDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
