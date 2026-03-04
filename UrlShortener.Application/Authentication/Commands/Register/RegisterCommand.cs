using MediatR;
using ErrorOr;
using UrlShortener.Application.Authentication.Common;

namespace UrlShortener.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Login,
    string Email, 
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;