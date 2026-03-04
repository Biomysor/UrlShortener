using MediatR;
using ErrorOr;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Domain.UrlAggregate;

namespace UrlShortener.Application.UrlActions.Commands;

public class ShortenUrlCommandHandler(
    IUrlRepository repository,
    IUrlCodeGenerator codeGenerator,
    IShortUrlBuilder shortUrlBuilder)
    : IRequestHandler<ShortenUrlCommand, ErrorOr<UrlResult>>
{
    private readonly IShortUrlBuilder _shortUrlBuilder =  shortUrlBuilder;
    private readonly IUrlRepository _repository = repository;
    private readonly IUrlCodeGenerator _codeGenerator = codeGenerator;

    public async Task<ErrorOr<UrlResult>> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByLongUrlAsync(request.Url);
        if (existing != null)
        {
            var shortUrl = _shortUrlBuilder.BuildShortUrl(existing.Code);
            return new UrlResult(existing.Id, shortUrl);
        }
            
        
        var url = new Url(request.Url);
        _repository.Add(url); 

        var code = _codeGenerator.GenerateCode(url.Id);
        url.SetCode(code);
        
        var newShortUrl = _shortUrlBuilder.BuildShortUrl(code);
        _repository.Update(url);

        return new UrlResult(url.Id, newShortUrl);
    }
}