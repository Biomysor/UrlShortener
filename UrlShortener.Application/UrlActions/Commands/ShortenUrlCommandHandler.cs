using ErrorOr;
using MediatR;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Domain.UrlAggregate.Entity;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.Application.UrlActions.Commands;

/// <summary>
///     Handles commands for creating short URLs.
///     Uses database storage, Redis cache and RabbitMQ messaging.
/// </summary>
public class ShortenUrlCommandHandler(
    IUrlRepository repository,
    IUrlCodeGenerator codeGenerator,
    IShortUrlBuilder shortUrlBuilder,
    ICacheService cacheService,
    IMessagePublisher messagePublisher)
    : IRequestHandler<ShortenUrlCommand, ErrorOr<UrlResult>>
{
    /// <summary>
    ///     Creates a new short URL or returns an existing one.
    ///     If a new URL is created, the method saves it to the database,
    ///     stores redirect data in cache and publishes a URL created event.
    /// </summary>
    /// <param name="request">Command containing the original long URL.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>UrlResult with URL identifier and generated short URL.</returns>
    public async Task<ErrorOr<UrlResult>> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByLongUrlAsync(request.Url, cancellationToken);
        if (existing != null)
        {
            var shortUrl = shortUrlBuilder.BuildShortUrl(existing.Code);

            await cacheService.SetAsync(
                $"url:code:{existing.Code}",
                new CachedUrlRedirect(
                    existing.Id.Value,
                    existing.Code,
                    existing.LongUrl),
                TimeSpan.FromHours(1),
                cancellationToken);

            return new UrlResult(existing.Id.Value, shortUrl, existing.CreatedAtUtc);
        }


        var url = Url.Create(request.Url);

        var code = codeGenerator.GenerateCode(url.Id);
        url.SetCode(code);

        await repository.AddAsync(url, cancellationToken);

        var newShortUrl = shortUrlBuilder.BuildShortUrl(code);

        await cacheService.SetAsync(
            $"url:code:{code}",
            new CachedUrlRedirect(
                url.Id.Value,
                url.Code,
                url.LongUrl),
            TimeSpan.FromHours(1),
            cancellationToken);

        var time = TimeZoneInfo.Local.GetUtcOffset(url.CreatedAtUtc);

        await messagePublisher.PublishAsync(
            new UrlCreatedEvent(
                url.Code,
                url.LongUrl,
                newShortUrl,
                url.CreatedAtUtc + time),
            cancellationToken);

        return new UrlResult(url.Id.Value, newShortUrl, url.CreatedAtUtc);
    }
}