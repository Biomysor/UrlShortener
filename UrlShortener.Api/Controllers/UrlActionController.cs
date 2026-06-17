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
    private readonly IMapper _mapper = mapper;
    private readonly ISender _sender = sender;

    [HttpGet("getUrls")]
    public async Task<IActionResult> GetUrls([FromQuery] UrlRequest request)
    {
        var query = _mapper.Map<UrlQuery>(request);
        var urlResult = await _sender.Send(query);

        return urlResult.Match(
            r => Ok(_mapper.Map<UrlResponse>(r)),
            errors => Problem(errors)
        );
    }

    [HttpPost("shortenUrl")]
    public async Task<IActionResult> ShortenUrl(UrlRequest request)
    {
        var command = _mapper.Map<ShortenUrlCommand>(request);
        var urlResult = await _sender.Send(command);

        return urlResult.Match(
            r => Ok(_mapper.Map<UrlResponse>(r)),
            errors => Problem(errors)
        );
    }
}