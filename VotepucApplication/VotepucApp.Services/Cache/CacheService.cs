using Microsoft.Extensions.Caching.Distributed;

namespace VotepucApp.Services.Cache;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };
    }
    public async Task SetAsync(string key, string value)
    {
        await _distributedCache.SetStringAsync(key, value, _distributedCacheEntryOptions);
    }

    public async Task<string> GetAsync(string key)
    {
        return await _distributedCache.GetStringAsync(key);
    }
}