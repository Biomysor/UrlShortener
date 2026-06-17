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
    private readonly ICacheService _cacheService = cacheService;
    private readonly IUrlCodeGenerator _codeGenerator = codeGenerator;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;
    private readonly IUrlRepository _repository = repository;
    private readonly IShortUrlBuilder _shortUrlBuilder = shortUrlBuilder;

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
        var existing = await _repository.GetByLongUrlAsync(request.Url);
        if (existing != null)
        {
            var shortUrl = _shortUrlBuilder.BuildShortUrl(existing.Code);

            await _cacheService.SetAsync(
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

        var code = _codeGenerator.GenerateCode(url.Id);
        url.SetCode(code);

        await _repository.AddAsync(url);

        var newShortUrl = _shortUrlBuilder.BuildShortUrl(code);

        await _cacheService.SetAsync(
            $"url:code:{code}",
            new CachedUrlRedirect(
                url.Id.Value,
                url.Code,
                url.LongUrl),
            TimeSpan.FromHours(1),
            cancellationToken);

        var time = TimeZoneInfo.Local.GetUtcOffset(url.CreatedAtUtc);

        await _messagePublisher.PublishAsync(
            new UrlCreatedEvent(
                url.Id.Value,
                url.Code,
                url.LongUrl,
                newShortUrl,
                url.CreatedAtUtc + time),
            cancellationToken);

        return new UrlResult(url.Id.Value, newShortUrl, url.CreatedAtUtc);
    }
}