using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class UserSettingsRepository(NotificationDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<UserSettings, NotificationDbContext>(dbContext, cacheBroker), IUserSettingsRepository
{
    public new ValueTask<UserSettings?> GetByIdAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(userId, asNoTracking, cancellationToken);
}