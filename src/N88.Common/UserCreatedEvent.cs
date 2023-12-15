namespace N88.Common;

public class UserCreatedEvent(Guid userId) : IdentityEvent(userId);