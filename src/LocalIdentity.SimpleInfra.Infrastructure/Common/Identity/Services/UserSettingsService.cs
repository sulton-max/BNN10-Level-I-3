using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class UserSettingsService(IUserSettingsRepository userSettingsRepository) : IUserSettingsService
{
    public ValueTask<UserSettings?> GetByIdAsync(Guid userSettingsId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userSettingsRepository.GetByIdAsync(userSettingsId, asNoTracking, cancellationToken);

    public ValueTask<UserSettings> CreateAsync(UserSettings userSettings, bool saveChanges = true, CancellationToken cancellationToken = default)
        => userSettingsRepository.CreateAsync(userSettings, saveChanges, cancellationToken);
}