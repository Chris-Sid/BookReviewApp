using BookReviewApp.Entities.Models;

namespace BookReviewApp.WebUI.Models
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
    }

    public class ReviewViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public string Username { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public bool? UserVoted { get; set; } // null = not voted, true = liked, false = disliked
    }

}
