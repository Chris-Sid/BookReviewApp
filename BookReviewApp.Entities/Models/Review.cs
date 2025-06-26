using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Entities.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Content { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<ReviewVote> Votes { get; set; }
    }
}
