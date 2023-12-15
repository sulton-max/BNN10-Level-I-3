namespace N88.Common;

public abstract class IdentityEvent(Guid userId) : Event
{
    public Guid UserId { get; set; } = userId;
}