using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Common.Caching;
using LocalIdentity.SimpleInfra.Domain.Common.Entities;
using LocalIdentity.SimpleInfra.Domain.Common.Query;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

/// <summary>
///     Provides base functionality for entity repositories
/// </summary>
/// <typeparam name="TEntity">Type of entity</typeparam>
/// <typeparam name="TContext">Type of context</typeparam>
public class EntityRepositoryBase<TEntity, TContext>(TContext dbContext, ICacheBroker cacheBroker, CacheEntryOptions? cacheEntryOptions = default)
    where TEntity : class, IEntity where TContext : DbContext
{
    protected TContext DbContext => dbContext;

    protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = default, bool asNoTracking = false)
    {
        var initialQuery = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
            initialQuery = initialQuery.Where(predicate);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return initialQuery;
    }


    /// <summary>
    ///     Retrieves a list of entities based on query specification.
    /// </summary>
    /// <param name="querySpecification">The query specification to apply.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of entities that match the given query specification.</returns>
    protected async ValueTask<IList<TEntity>> GetAsync(QuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default)
    {
        var foundEntities = new List<TEntity>();
        var cacheKey = querySpecification.CacheKey;

        if (cacheEntryOptions is null || !await cacheBroker.TryGetAsync<List<TEntity>>(cacheKey, out var cachedEntities, cancellationToken))
        {
            var initialQuery = DbContext.Set<TEntity>().AsQueryable();

            if (querySpecification.AsNoTracking) initialQuery = initialQuery.AsNoTracking();
            initialQuery = initialQuery.ApplySpecification(querySpecification);
            foundEntities = await initialQuery.ToListAsync(cancellationToken);
            if (cacheEntryOptions is not null) await cacheBroker.SetAsync(cacheKey, foundEntities, cacheEntryOptions, cancellationToken);
        }
        else if (cachedEntities is not null)
        {
            foundEntities = cachedEntities;
        }

        return foundEntities;
    }

    protected async ValueTask<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var initialQuery = DbContext.Set<TEntity>().Where(entity => true);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return await initialQuery.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    protected async ValueTask<TEntity> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        if (cacheEntryOptions is not null) await cacheBroker.SetAsync(entity.Id.ToString(), entity, cacheEntryOptions, cancellationToken);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }


    protected async ValueTask<TEntity> UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);

        if (saveChanges)
            await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}