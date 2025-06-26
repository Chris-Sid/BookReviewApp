using System.ComponentModel.DataAnnotations;

namespace BookReviewApp.WebUI.ViewModels
{
    public class EditBookViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int PublishedYear { get; set; }

        public string Genre { get; set; }
    }
}
