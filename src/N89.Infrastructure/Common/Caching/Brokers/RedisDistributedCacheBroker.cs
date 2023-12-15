using AirBnb.Infrastructure.Common.Settings;
using AirBnb.Persistence.Caching.Brokers;
using AirBnb.Persistence.Caching.Models;
using Force.DeepCloner;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AirBnb.Infrastructure.Common.Caching.Brokers;

/// <summary>
/// Provides functionality of Redis cache broker for distributed caching
/// </summary>
public class RedisDistributedCacheBroker(IOptions<CacheSettings> cacheSettings, IDistributedCache distributedCache) : ICacheBroker
{
    private readonly DistributedCacheEntryOptions _entryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSettings.Value.AbsoluteExpirationInSeconds),
        SlidingExpiration = TimeSpan.FromSeconds(cacheSettings.Value.SlidingExpirationInSeconds)
    };

    public ValueTask<bool> TryGetAsync<T>(string key, out T? value)
    {
        var foundEntry = distributedCache.GetString(key);

        if (foundEntry is not null)
        {
            value = JsonConvert.DeserializeObject<T>(foundEntry);
            return ValueTask.FromResult(true);
        }

        value = default;
        return ValueTask.FromResult(false);
    }

    public async ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default)
    {
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), GetCacheEntryOptions(entryOptions));
    }

    /// <summary>
    /// Gets the cache entry options based on given entry options or default options.
    /// </summary>
    /// <param name="entryOptions">Given cache entry options.</param>
    /// <returns>The distributed cache entry options.</returns>
    private DistributedCacheEntryOptions GetCacheEntryOptions(CacheEntryOptions? entryOptions)
    {
        if (entryOptions == default || (!entryOptions.AbsoluteExpirationRelativeToNow.HasValue && !entryOptions.SlidingExpiration.HasValue))
            return _entryOptions;

        var currentEntryOptions = _entryOptions.DeepClone();

        currentEntryOptions.AbsoluteExpirationRelativeToNow = entryOptions.AbsoluteExpirationRelativeToNow;
        currentEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration;

        return currentEntryOptions;
    }
}