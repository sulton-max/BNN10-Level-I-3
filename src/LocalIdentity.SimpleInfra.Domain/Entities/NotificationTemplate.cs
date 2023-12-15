using LocalIdentity.SimpleInfra.Domain.Common.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public abstract class NotificationTemplate : IEntity
{
    public Guid Id { get; set; }
    
    public NotificationType Type { get; set; }
    
    public NotificationTemplateType TemplateType { get; set; }

    public string Content { get; set; } = default!;

    public IList<NotificationHistory> Histories { get; set; } = new List<NotificationHistory>();
}