using System.Net.Security;
using MediatR;
using ErrorOr;
using MapsterMapper;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate;

namespace UrlShortener.Application.Authentication.Queries;

public class LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // If user exist

        if (userRepository.GetByEmail(query.Email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }
        // password
        if (user.Password != query.Password)
        {
            return new[] {Errors.Authentication.InvalidCredentials};
        }

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}