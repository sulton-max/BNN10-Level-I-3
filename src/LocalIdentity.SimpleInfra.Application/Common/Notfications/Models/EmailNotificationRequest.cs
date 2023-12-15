using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

public class EmailNotificationRequest : NotificationRequest
{
    public EmailNotificationRequest() => Type = NotificationType.Email;
}