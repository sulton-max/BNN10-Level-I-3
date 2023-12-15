namespace LocalIdentity.SimpleInfra.Application.Common.Verifications.Services;

public interface IVerificationProcessingService
{
    ValueTask<bool> Verify(string code, CancellationToken cancellationToken);
}