using AirBnb.Persistence.Caching.Models;

namespace AirBnb.Persistence.Caching.Brokers;

/// <summary>
/// Defines cache broker functionality
/// </summary>
public interface ICacheBroker
{
    /// <summary>
    /// Retrieves the cache entry with given key if exists otherwise returns null.
    /// </summary>
    /// <param name="key">The cache key of entry.</param>
    /// <param name="value">Cache entry to be stored.</param>
    /// <typeparam name="T">The type of the value to be stored.</typeparam>
    /// <returns> Cache entry with given key if exists, otherwise null. </returns>
    ValueTask<bool> TryGetAsync<T>(string key, out T? value);

    /// <summary>
    /// Sets the value of an item in the cache.
    /// </summary>
    /// <param name="key">The cache key for entry.</param>
    /// <param name="value">Cache entry to be stored.</param>
    /// <param name="entryOptions">The cache entry options.</param>
    /// <typeparam name="T">The type of the value to be stored.</typeparam>
    ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default);
}