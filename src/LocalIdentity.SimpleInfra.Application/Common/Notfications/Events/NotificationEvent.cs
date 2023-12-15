using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Domain.Common.Events;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Events;

public class NotificationEvent : Event
{
    public NotificationRequest NotificationContext { get; set; } = default!;
}