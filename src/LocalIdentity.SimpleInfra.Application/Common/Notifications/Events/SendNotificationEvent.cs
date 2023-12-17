using LocalIdentity.SimpleInfra.Application.Common.Notifications.Models;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;

public class SendNotificationEvent : NotificationEvent
{
    public NotificationMessage Message { get; set; } = default!;
}