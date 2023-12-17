using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class UserSettingsRepository(IdentityDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<UserSettings, IdentityDbContext>(dbContext, cacheBroker), IUserSettingsRepository
{
    public new ValueTask<UserSettings?> GetByIdAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return base.GetByIdAsync(userId, asNoTracking, cancellationToken);
    }

    public new ValueTask<UserSettings> CreateAsync(UserSettings userSettings, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(userSettings, saveChanges, cancellationToken);
    }
}