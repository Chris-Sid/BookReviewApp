using System;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int PublishedYear { get; set; }
    public string Genre { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
