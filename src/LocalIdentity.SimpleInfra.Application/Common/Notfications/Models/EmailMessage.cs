using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

public class EmailMessage : NotificationMessage
{
    public string SendEmailAddress { get; set; } = default!;

    public string ReceiverEmailAddress { get; set; } = default!;

    public EmailTemplate Template { get; set; } = default!;

    public string Subject { get; set; } = default!;

    public string Body { get; set; } = default!;
}