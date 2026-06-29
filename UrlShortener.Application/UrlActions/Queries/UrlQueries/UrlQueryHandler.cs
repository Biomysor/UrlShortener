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
    public async Task<ErrorOr<UrlResult>> Handle(UrlQuery request, CancellationToken cancellationToken)
    {
        var url = await repository.GetByLongUrlAsync(request.Url, cancellationToken);

        if (url is null)
            return Error.NotFound(
                "Url.NotFound",
                "Url not found"
            );

        var shortUrl = shortUrlBuilder.BuildShortUrl(url.Code);

        return new UrlResult(
            url.Id.Value,
            shortUrl,
            url.CreatedAtUtc
        );
    }
}