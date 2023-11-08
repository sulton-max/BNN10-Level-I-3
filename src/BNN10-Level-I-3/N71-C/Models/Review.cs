namespace N71_C.Models;

public class Review
{
    public Guid Id { get; set; }
    
    public Guid BookId { get; set; }
    
    public int Rating { get; set; }
}