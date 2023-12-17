namespace N88.Common;

public abstract class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;
}