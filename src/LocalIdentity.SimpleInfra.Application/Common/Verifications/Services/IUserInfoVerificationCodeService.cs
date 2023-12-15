using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Verifications.Services;

public interface IUserInfoVerificationCodeService : IVerificationCodeService
{
    IList<string> Generate();

    ValueTask<(UserInfoVerificationCode Code, bool IsValid)> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    ValueTask<UserInfoVerificationCode> CreateAsync(VerificationCodeType codeType, Guid userId, CancellationToken cancellationToken = default);

    ValueTask DeactivateAsync(Guid codeId, bool saveChanges = true, CancellationToken cancellationToken = default);
}