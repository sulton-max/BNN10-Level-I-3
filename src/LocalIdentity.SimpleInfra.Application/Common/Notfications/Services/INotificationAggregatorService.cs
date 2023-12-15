using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Domain.Common.Exceptions;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;

public interface INotificationAggregatorService
{
    ValueTask<FuncResult<bool>> SendAsync(
        NotificationRequest notificationRequest,
        CancellationToken cancellationToken = default
    );
    
    ValueTask<IList<NotificationTemplate>> GetTemplatesByFilterAsync(
        NotificationTemplateFilter filter,
        CancellationToken cancellationToken = default
    );
}