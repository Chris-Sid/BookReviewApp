using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Contracts.DTOs
{
    public class AddReviewDto
    {
        [Required]
        public Guid BookId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Content { get; set; } = default!;
    }
}
