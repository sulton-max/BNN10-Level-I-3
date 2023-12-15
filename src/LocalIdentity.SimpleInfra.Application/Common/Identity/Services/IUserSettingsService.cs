using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IUserSettingsService
{
    ValueTask<UserSettings?> GetByIdAsync(Guid userSettingsId, bool asNoTracking = false, CancellationToken cancellationToken = default);
}