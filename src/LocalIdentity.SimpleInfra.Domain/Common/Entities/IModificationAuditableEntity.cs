namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface IModificationAuditableEntity
{
    Guid? ModifiedByUserId { get; set; }
}