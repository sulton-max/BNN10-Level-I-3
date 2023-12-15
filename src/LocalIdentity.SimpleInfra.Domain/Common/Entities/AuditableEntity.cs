namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public abstract class AuditableEntity : SoftDeletedEntity, IAuditableEntity
{
    public DateTimeOffset CreatedTime { get; set; }

    public DateTimeOffset? ModifiedTime { get; set; }
}