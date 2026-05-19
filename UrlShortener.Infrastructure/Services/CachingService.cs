using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Application.Common.Interfaces.Services;

namespace UrlShortener.Infrastructure.Services;

public class CachingService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        var json = await cache.GetStringAsync(key, cancellationToken);

        return json is null
            ? default
            : JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
        };

        await cache.SetStringAsync(key, json, options, cancellationToken);
    }

    public async Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }
}