namespace LocalIdentity.SimpleInfra.Domain.Common.Caching;

public class CacheEntryOptions
{
    public CacheEntryOptions()
    {
    }

    public CacheEntryOptions(TimeSpan? absoluteExpirationRelativeToNow, TimeSpan? slidingExpiration)
    {
        AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
        SlidingExpiration = slidingExpiration;
    }

    public TimeSpan? AbsoluteExpirationRelativeToNow { get; init; }
    public TimeSpan? SlidingExpiration { get; init; }
}