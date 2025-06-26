using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Contracts.Models
{
    public class BookFilterDto
    {
        public string? Genre { get; set; }
        public int? Year { get; set; }
        public int? MinRating { get; set; }
    }
}
