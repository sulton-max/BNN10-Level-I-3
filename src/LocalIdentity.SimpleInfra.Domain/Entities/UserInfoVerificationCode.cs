using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class UserInfoVerificationCode : VerificationCode
{
    public UserInfoVerificationCode()
    {
        Type = VerificationType.UserInfoVerificationCode;
    }

    public Guid UserId { get; set; }
}