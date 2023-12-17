using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;

public class ProcessNotificationEvent : NotificationEvent
{
    public NotificationTemplateType TemplateType { get; init; }

    public NotificationType? Type { get; set; }

    public Dictionary<string, string>? Variables { get; set; }
}