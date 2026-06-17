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
    private readonly IMapper _mapper = mapper;
    private readonly ISender _sender = sender;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var authResult = await _sender.Send(command, cancellationToken);
        
        return authResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponce>(result)),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authResult = await _sender.Send(query, cancellationToken);
        
        return authResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponce>(result)),
            errors => Problem(errors));
    }
}