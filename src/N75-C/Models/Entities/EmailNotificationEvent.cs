using N75_C.Models.Common;

namespace N75_C.Models.Entities;

public class EmailNotificationEvent : NotificationEvent
{
    public string Subject { get; set; } = default!;

    public string ReceiverEmailAddress { get; set; } = default!;
}