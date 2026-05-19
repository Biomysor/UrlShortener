using MediatR;
using ErrorOr;
using UrlShortener.Application.Authentication.Common;

namespace UrlShortener.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
