using LocalIdentity.SimpleInfra.Domain.Common.Query;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

public class NotificationTemplateFilter : FilterPagination
{
    public IList<NotificationType> TemplateType { get; set; }
}