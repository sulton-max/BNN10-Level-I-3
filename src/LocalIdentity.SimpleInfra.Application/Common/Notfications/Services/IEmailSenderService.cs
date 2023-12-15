using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;

public interface IEmailSenderService
{
    ValueTask<bool> SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}