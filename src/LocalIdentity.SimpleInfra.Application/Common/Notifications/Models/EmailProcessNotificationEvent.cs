using LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Models;

public class EmailProcessNotificationEvent : ProcessNotificationEvent
{
    public EmailProcessNotificationEvent()
    {
        Type = NotificationType.Email;
    }
}