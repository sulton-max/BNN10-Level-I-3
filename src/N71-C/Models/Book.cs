namespace N71_C.Models;

public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int Pages { get; set; }

    public Guid AuthorId { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}