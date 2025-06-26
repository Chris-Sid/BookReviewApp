using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Entities.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; } = null!;
        [Required, StringLength(100)]
        public string Author { get; set; } = null!;
        [Range(1450, 2025, ErrorMessage = "Enter a valid year.")]
        public int PublishedYear { get; set; }
        [Required, StringLength(50)]
        public string Genre { get; set; } = null!;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
