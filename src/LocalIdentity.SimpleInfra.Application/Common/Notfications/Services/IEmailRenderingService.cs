using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;

public interface IEmailRenderingService
{
    ValueTask<string> RenderAsync(
        EmailMessage emailMessage,
        // string template,
        // Dictionary<string, string> variables,
        CancellationToken cancellationToken = default
    );
}