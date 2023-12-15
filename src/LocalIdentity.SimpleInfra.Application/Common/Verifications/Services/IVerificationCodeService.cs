using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Verifications.Services;

public interface IVerificationCodeService
{
    ValueTask<VerificationType?> GetVerificationTypeAsync(string code, CancellationToken cancellationToken = default);
}