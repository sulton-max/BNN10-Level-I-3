using Type = LocalIdentity.SimpleInfra.Domain.Enums.NotificationType;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class EmailTemplate : NotificationTemplate
{
    public EmailTemplate()
    {
        Type = Type.Email;
    }

    public string Subject { get; set; } = default!;
}