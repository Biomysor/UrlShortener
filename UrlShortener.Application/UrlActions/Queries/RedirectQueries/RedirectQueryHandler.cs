using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.Application.UrlActions.Queries.RedirectQueries;

/// <summary>
///     Handles redirect queries by short URL code.
///     Uses Redis cache to speed up redirects and publishes redirect events for analytics.
/// </summary>
public class RedirectQueryHandler(
    IUrlRepository repository,
    ICacheService cacheService,
    IMessagePublisher messagePublisher,
    ILogger<RedirectQueryHandler> logger) : IRequestHandler<RedirectQuery, ErrorOr<string>>
{
    /// <summary>
    ///     Finds the original long URL by short code.
    ///     The method first checks cache. If the value is not cached,
    ///     it loads URL data from the database, caches it and publishes a redirect event.
    /// </summary>
    /// <param name="request">Redirect query containing short URL code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Original long URL or NotFound error.</returns>
    public async Task<ErrorOr<string>> Handle(RedirectQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"url:code:{request.Code}";


        var cachedUrl = await cacheService.GetAsync<CachedUrlRedirect>(
            cacheKey,
            cancellationToken);

        if (cachedUrl is not null)
        {
            await PublishRedirectEventAsync(cachedUrl.UrlId, cachedUrl.Code, cachedUrl.LongUrl, cancellationToken);

            return cachedUrl.LongUrl;
        }

        var url = await repository.GetCodeAsync(request.Code, cancellationToken);

        if (url is null) return Error.NotFound("404", "Url Not found");
        
        await PublishRedirectEventAsync(url.Id.Value, url.Code, url.LongUrl, cancellationToken);

        await cacheService.SetAsync(
            cacheKey,
            new CachedUrlRedirect(
                url.Id.Value,
                url.Code,
                url.LongUrl),
            TimeSpan.FromHours(1),
            cancellationToken);


        return url.LongUrl;
    }

    private async Task PublishRedirectEventAsync(Guid urlId, string code, string longUrl,
        CancellationToken cancellationToken)
    {
        try
        {
            await messagePublisher.PublishAsync(
                new UrlRedirectedEvent(
                    urlId,
                    code,
                    longUrl,
                    DateTime.UtcNow,
                    null,
                    null),
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Failed to publish redirect event for code {Code}.", code);
        }
    }
}