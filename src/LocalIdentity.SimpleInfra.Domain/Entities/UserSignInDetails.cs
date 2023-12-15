using LocalIdentity.SimpleInfra.Domain.Common;
using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class UserSignInDetails : AuditableEntity
{
    public string IpAddress { get; set; } = default!;

    public string UserAgent { get; set; } = default!;
    
    public Guid UserId { get; set; }
}