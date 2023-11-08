namespace N71_C.Models;

public class Author
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public int Age { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}