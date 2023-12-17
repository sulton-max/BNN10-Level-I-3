using LocalIdentity.SimpleInfra.Application.Common.Notifications.Models;
using LocalIdentity.SimpleInfra.Domain.Common.Exceptions;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Services;

public interface IEmailOrchestrationService
{
    ValueTask<FuncResult<bool>> SendAsync(EmailProcessNotificationEvent @event, CancellationToken cancellationToken = default);
}