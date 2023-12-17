using LocalIdentity.SimpleInfra.Domain.Entities;
using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Models;

public class EmailMessage : NotificationMessage
{
    public string ReceiverEmailAddress { get; set; } = default!;

    public string SenderEmailAddress { get; set; } = default!;

    public string Subject { get; set; } = default!;

    [JsonIgnore]
    public EmailTemplate EmailTemplate
    {
        get => (EmailTemplate)Template;
        set => Template = value;
    }
}