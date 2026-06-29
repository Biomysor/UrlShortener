using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Application.Common.Interfaces.Services;

namespace UrlShortener.Infrastructure.Services;

/// <summary>
/// Redis implementation of the cache service.
/// Uses IDistributedCache and JSON serialization for storing objects.
/// </summary>
public class CachingService(IDistributedCache cache) : ICacheService
{
    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        var json = await cache.GetStringAsync(key, cancellationToken);

        return json is null
            ? default
            : JsonSerializer.Deserialize<T>(json);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }
}