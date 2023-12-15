namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface IAuditableEntity : ISoftDeletedEntity
{
    DateTimeOffset CreatedTime { get; set; }
    
    DateTimeOffset? ModifiedTime { get; set; }
}