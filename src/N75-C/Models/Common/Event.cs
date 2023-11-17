namespace N75_C.Models.Common;

public abstract class Event
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public bool IsCancelled { get; set; }
}