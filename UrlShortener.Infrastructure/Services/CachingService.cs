using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Application.Common.Interfaces.Services;

namespace UrlShortener.Infrastructure.Services;

public class CachingService: ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CachingService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<string?> GetAsync(string key, CancellationToken token = default)
    {
        return await _distributedCache.GetStringAsync(key, token);
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiration = null, CancellationToken token = default)
    { 
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
        };
        
        await _distributedCache.SetStringAsync(key, value, options, token);
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await _distributedCache.RemoveAsync(key, token);
    }
}