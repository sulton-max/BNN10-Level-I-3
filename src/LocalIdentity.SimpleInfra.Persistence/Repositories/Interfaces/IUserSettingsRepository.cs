using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

public interface IUserSettingsRepository
{
    ValueTask<UserSettings?> GetByIdAsync(
        Guid userId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}