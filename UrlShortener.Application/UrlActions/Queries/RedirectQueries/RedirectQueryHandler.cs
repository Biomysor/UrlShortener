using MediatR;
using ErrorOr;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UrlAggregate;

namespace UrlShortener.Application.UrlActions.Queries.RedirectQueries;

public class RedirectQueryHandler(IUrlRepository repository, ICacheService cacheService) : IRequestHandler<RedirectQuery, ErrorOr<string>>
{
    private  readonly IUrlRepository _repository = repository;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<ErrorOr<string>> Handle(RedirectQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"url:code:{request.Code}";
        
        var cachedLongUrl = await _cacheService.GetAsync(
            cacheKey,
            cancellationToken);

        if (cachedLongUrl is not null)
        {
            return cachedLongUrl;
        }
        
        var url = await _repository.GetCodeAsync(request.Code);

        if (url is null)
        {
            return Error.NotFound("404", "Url Not found");
        }
        
        await _cacheService.SetAsync(
            cacheKey,
            url.LongUrl,
            TimeSpan.FromHours(1),
            cancellationToken);
        
        return url.LongUrl;
    }
}