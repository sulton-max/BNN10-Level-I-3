using LocalIdentity.SimpleInfra.Domain.Common.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class UserSettings : IEntity
{
    /// <summary>
    /// Gets or sets the user Id
    /// </summary>
    public Guid Id { get; set; }

    public NotificationType? PreferredNotificationType { get; set; }
}