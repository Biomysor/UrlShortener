namespace UrlShortener.Application.Common.Interfaces.Services;

public interface ICacheService
{
    Task<string?> GetAsync(string key, CancellationToken token = default);
    
    Task SetAsync(string key, string value, TimeSpan? expiration = null, CancellationToken token = default);
    
    Task RemoveAsync(string key, CancellationToken token = default);
}