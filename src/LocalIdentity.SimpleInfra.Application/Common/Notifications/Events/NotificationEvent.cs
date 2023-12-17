using LocalIdentity.SimpleInfra.Domain.Common.Events;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;

public class NotificationEvent : Event
{
    public Guid SenderUserId { get; init; }

    public Guid ReceiverUserId { get; init; }
}