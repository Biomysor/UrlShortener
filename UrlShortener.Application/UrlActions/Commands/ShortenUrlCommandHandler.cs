using MediatR;
using ErrorOr;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Domain.UrlAggregate;
using UrlShortener.Domain.UrlAggregate.Entity;

namespace UrlShortener.Application.UrlActions.Commands;

public class ShortenUrlCommandHandler(
    IUrlRepository repository,
    IUrlCodeGenerator codeGenerator,
    IShortUrlBuilder shortUrlBuilder)
    : IRequestHandler<ShortenUrlCommand, ErrorOr<UrlResult>>
{
    private readonly IShortUrlBuilder _shortUrlBuilder = shortUrlBuilder;
    private readonly IUrlRepository _repository = repository;
    private readonly IUrlCodeGenerator _codeGenerator = codeGenerator;

    public async Task<ErrorOr<UrlResult>> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByLongUrlAsync(request.Url);
        if (existing != null)
        {
            var shortUrl = _shortUrlBuilder.BuildShortUrl(existing.Code);
            return new UrlResult(existing.Id.Value, shortUrl);
        }


        var url = Url.Create(request.Url);

        var code = _codeGenerator.GenerateCode(url.Id);
        url.SetCode(code);
        
        await _repository.AddAsync(url);

        var newShortUrl = _shortUrlBuilder.BuildShortUrl(code);

        return new UrlResult(url.Id.Value, newShortUrl);
    }
}