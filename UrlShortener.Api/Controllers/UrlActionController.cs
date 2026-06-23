using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.UrlActions.Commands;
using UrlShortener.Application.UrlActions.Queries.UrlQueries;
using UrlShortener.Contracts.UrlAction;

namespace UrlShortener.Api.Controllers;

[Route("url")]
[AllowAnonymous]
public class UrlActionController(ISender sender, IMapper mapper) : ApiController
{
    [HttpGet("getUrls")]
    public async Task<IActionResult> GetUrls([FromQuery] UrlRequest request, CancellationToken cancellationToken)
    {
        var query = mapper.Map<UrlQuery>(request);
        var urlResult = await sender.Send(query, cancellationToken);

        return urlResult.Match(
            r => Ok(mapper.Map<UrlResponse>(r)),
            Problem
        );
    }

    [HttpPost("shortenUrl")]
    public async Task<IActionResult> ShortenUrl(UrlRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<ShortenUrlCommand>(request);
        var urlResult = await sender.Send(command, cancellationToken);

        return urlResult.Match(
            r => Ok(mapper.Map<UrlResponse>(r)),
            Problem
        );
    }
}