using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IUserSignInDetailsService
{
    ValueTask<bool> ValidateSignInLocation(CancellationToken cancellationToken = default);

    ValueTask<UserSignInDetails?> GetLastSignInDetailsAsync(Guid userId, bool asNoTracking, CancellationToken cancellationToken = default);

    ValueTask RecordSignInAsync(bool saveChanges = true, CancellationToken cancellationToken = default);
}