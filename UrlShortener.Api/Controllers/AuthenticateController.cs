using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Authentication.Commands.Register;
using UrlShortener.Contracts.Authentication;
using UrlShortener.Application.Authentication.Queries;

namespace UrlShortener.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticateController(IMapper mapper, ISender sender ) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterCommand>(request);
        var authResult = await sender.Send(command, cancellationToken);
        
        return authResult.Match(
            result => Ok(mapper.Map<AuthenticationResponce>(result)),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var query = mapper.Map<LoginQuery>(request);
        var authResult = await sender.Send(query, cancellationToken);
        
        return authResult.Match(
            result => Ok(mapper.Map<AuthenticationResponce>(result)),
            Problem);
    }
}