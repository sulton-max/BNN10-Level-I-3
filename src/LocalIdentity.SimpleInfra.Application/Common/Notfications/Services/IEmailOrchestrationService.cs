using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Domain.Common.Exceptions;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;

public interface IEmailOrchestrationService
{
    ValueTask<FuncResult<bool>> SendAsync(
        EmailNotificationRequest request,
        CancellationToken cancellationToken = default
    );
}