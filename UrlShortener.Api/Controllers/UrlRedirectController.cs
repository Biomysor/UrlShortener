using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.UrlActions.Queries.RedirectQueries;

namespace UrlShortener.Api.Controllers;

[AllowAnonymous]
public class UrlRedirectController(ISender sender) : ApiController
{
    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectToLongUrl(string code, CancellationToken cancellationToken)
    {
        var query = new RedirectQuery(code);
        var result = await sender.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            longUrl => base.Redirect(longUrl),
            Problem);
    }
}