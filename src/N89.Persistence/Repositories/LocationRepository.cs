using AirBnb.Domain.Common.Query;
using AirBnb.Domain.Entities;
using AirBnb.Persistence.Caching.Brokers;
using AirBnb.Persistence.Caching.Models;
using AirBnb.Persistence.DataContexts;
using AirBnb.Persistence.Repositories.Interfaces;

namespace AirBnb.Persistence.Repositories;

/// <summary>
/// Represents a repository for managing locations in the application.
/// </summary>
public class LocationRepository(AppDbContext dbContext, ICacheBroker cacheBroker) : EntityRepositoryBase<Location, AppDbContext>(
    dbContext,
    cacheBroker,
    new CacheEntryOptions()
), ILocationRepository
{
    public new ValueTask<IList<Location>> GetAsync(QuerySpecification<Location> querySpecification, CancellationToken cancellationToken = default)
    {
        return base.GetAsync(querySpecification, cancellationToken);
    }
}