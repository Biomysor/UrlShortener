using MediatR;
using ErrorOr;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UrlAggregate;

namespace UrlShortener.Application.UrlActions.Queries.RedirectQueries;

public class RedirectQueryHandler(IUrlRepository repository) : IRequestHandler<RedirectQuery, ErrorOr<string>>
{
    private  readonly IUrlRepository _repository = repository;

    public async Task<ErrorOr<string>> Handle(RedirectQuery request, CancellationToken cancellationToken)
    {
        var url = await _repository.GetCodeAsync(request.Code);

        if (url is null)
        {
            return Error.NotFound("404", "Url Not found");
        }
        
        return url.LongUrl;
    }
}