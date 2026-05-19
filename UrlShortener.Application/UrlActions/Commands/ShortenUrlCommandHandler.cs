using MediatR;
using ErrorOr;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Domain.UrlAggregate.Entity;
using UrlShortener.Messaging.Contracts;
using UrlShortener.Messaging.Contracts.Events;

namespace UrlShortener.Application.UrlActions.Commands;

public class ShortenUrlCommandHandler(
    IUrlRepository repository,
    IUrlCodeGenerator codeGenerator,
    IShortUrlBuilder shortUrlBuilder,
    ICacheService cacheService,
    IMessagePublisher messagePublisher)
    : IRequestHandler<ShortenUrlCommand, ErrorOr<UrlResult>>
{
    private readonly IMessagePublisher _messagePublisher = messagePublisher;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IShortUrlBuilder _shortUrlBuilder = shortUrlBuilder;
    private readonly IUrlRepository _repository = repository;
    private readonly IUrlCodeGenerator _codeGenerator = codeGenerator;

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
            
            return new UrlResult(existing.Id.Value, shortUrl);
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
    
        await _messagePublisher.PublishAsync(
            new UrlCreatedEvent(
                url.Id.Value,
                url.Code,
                url.LongUrl,
                newShortUrl,
                url.CreatedAt),
            cancellationToken);

        return new UrlResult(url.Id.Value, newShortUrl);
    }
}