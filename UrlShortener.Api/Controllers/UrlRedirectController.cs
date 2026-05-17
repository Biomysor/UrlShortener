using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.UrlActions.Queries.RedirectQueries;

namespace UrlShortener.Api.Controllers;

[AllowAnonymous]
 public class UrlRedirectController(ISender sender) : ApiController
 {
     private readonly ISender _sender = sender;

     [HttpGet("/{code}")]
      public async Task<IActionResult> RedirectToLongUrl(string code)
     {
         var query = new RedirectQuery(code);
         var result = await _sender.Send(query);
         
         return result.Match<IActionResult>(
             longUrl => base.Redirect(longUrl),
             errors => Problem(errors));
     }
 }