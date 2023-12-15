using System.ComponentModel.DataAnnotations.Schema;
using LocalIdentity.SimpleInfra.Domain.Common;
using LocalIdentity.SimpleInfra.Domain.Common.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class User : AuditableEntity, IDeletionAuditableEntity, IModificationAuditableEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public int Age { get; set; }

    public string EmailAddress { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;
    
    public bool IsEmailAddressVerified { get; set; }
    
    public Guid? DeletedByUserId { get; set; }
    
    public Guid? ModifiedByUserId { get; set; }
    
    public RoleType Role { get; set; }
    
    public UserSettings UserSettings { get; set; }
}