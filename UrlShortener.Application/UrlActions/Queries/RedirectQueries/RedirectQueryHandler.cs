using MediatR;
using ErrorOr;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.Application.UrlActions.Queries.RedirectQueries;

public class RedirectQueryHandler(
    IUrlRepository repository,
    ICacheService cacheService,
    IMessagePublisher messagePublisher) : IRequestHandler<RedirectQuery, ErrorOr<string>>
{
    private readonly IMessagePublisher _messagePublisher = messagePublisher;
    private readonly IUrlRepository _repository = repository;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<ErrorOr<string>> Handle(RedirectQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"url:code:{request.Code}";


        var cachedUrl = await _cacheService.GetAsync<CachedUrlRedirect>(
            cacheKey,
            cancellationToken);

        if (cachedUrl is not null)
        {
            await _messagePublisher.PublishAsync(
                new UrlRedirectedEvent(
                    cachedUrl.UrlId,
                    cachedUrl.Code,
                    cachedUrl.LongUrl,
                    DateTime.UtcNow,
                    null,
                    null),
                cancellationToken);

            return cachedUrl.LongUrl;
        }

        var url = await _repository.GetCodeAsync(request.Code);

        if (url is null)
        {
            return Error.NotFound("404", "Url Not found");
        }

        await _messagePublisher.PublishAsync(new UrlRedirectedEvent(
                url.Id.Value,
                url.Code,
                url.LongUrl,
                DateTime.UtcNow,
                null,
                null),
            cancellationToken);

        await _cacheService.SetAsync(
            cacheKey,
            new CachedUrlRedirect(
                url.Id.Value,
                url.Code,
                url.LongUrl),
            TimeSpan.FromHours(1),
            cancellationToken);


        return url.LongUrl;
    }
}