using N75_C.Models.Enums;

namespace N75_C.Models.Common;

public abstract class NotificationEvent : Event
{
    public Guid ReceiverUserId { get; set; }

    public string Content { get; set; } = default!;

    public bool IsSuccessful { get; set; }

    public int ResentAttempts { get; set; }
    
    public NotificationType Type { get; set; }

    public Dictionary<string, string> Variables { get; set; } = new();
}