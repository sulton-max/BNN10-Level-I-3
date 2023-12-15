using LocalIdentity.SimpleInfra.Domain.Common;
using LocalIdentity.SimpleInfra.Domain.Common.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class Role : AuditableEntity
{
    public RoleType Type { get; set; }

    public bool IsActive { get; set; }
}