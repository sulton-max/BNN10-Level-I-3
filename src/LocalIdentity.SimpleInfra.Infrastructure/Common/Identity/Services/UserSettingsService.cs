using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly IUserSettingsRepository _userSettingsRepository;

    public UserSettingsService(IUserSettingsRepository userSettingsRepository)
    {
        _userSettingsRepository = userSettingsRepository;
    }

    public ValueTask<UserSettings?> GetByIdAsync(
        Guid userSettingsId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) =>
        _userSettingsRepository.GetByIdAsync(userSettingsId, asNoTracking, cancellationToken);
}