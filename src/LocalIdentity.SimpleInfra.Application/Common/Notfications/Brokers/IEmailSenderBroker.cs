using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Brokers;

public interface IEmailSenderBroker
{
    ValueTask<bool> SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}