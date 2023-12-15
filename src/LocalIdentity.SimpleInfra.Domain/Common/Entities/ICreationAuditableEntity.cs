namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface ICreationAuditableEntity
{
    Guid CreatedByUserId { get; set; }
}