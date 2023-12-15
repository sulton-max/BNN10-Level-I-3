namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

public abstract class NotificationMessage
{
    public Guid SenderUserId { get; set; }

    public Guid ReceiverUserId { get; set; }

    public Dictionary<string, string> Variables { get; set; }

    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; }
}