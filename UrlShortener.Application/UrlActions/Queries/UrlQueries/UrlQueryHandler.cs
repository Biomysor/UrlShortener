using ErrorOr;
using MediatR;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Common;

namespace UrlShortener.Application.UrlActions.Queries.UrlQueries;

public class UrlQueryHandler(
    IUrlRepository repository,
    IShortUrlBuilder shortUrlBuilder) :
    IRequestHandler<UrlQuery, ErrorOr<UrlResult>>
{
    private readonly IUrlRepository _repository = repository;
    private readonly IShortUrlBuilder _shortUrlBuilder = shortUrlBuilder;

    public async Task<ErrorOr<UrlResult>> Handle(UrlQuery request, CancellationToken cancellationToken)
    {
        var url = await _repository.GetByLongUrlAsync(request.Url);

        if (url is null)
            return Error.NotFound(
                "Url.NotFound",
                "Url not found"
            );

        var shortUrl = _shortUrlBuilder.BuildShortUrl(url.Code);

        return new UrlResult(
            url.Id.Value,
            shortUrl,
            url.CreatedAtUtc
        );
    }
}