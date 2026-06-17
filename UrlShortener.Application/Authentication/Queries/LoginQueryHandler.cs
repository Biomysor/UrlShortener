using ErrorOr;
using MediatR;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Authentication.Queries;

/// <summary>
///     Handles user login queries.
///     Validates user credentials and generates a JWT token.
/// </summary>
public class LoginQueryHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    /// <summary>
    ///     Processes a login request.
    /// </summary>
    /// <param name="query">Login query containing email and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    ///     Authentication result with JWT token if credentials are valid;
    ///     otherwise InvalidCredentials error.
    /// </returns>
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // If user exist

        if (await userRepository.GetByEmailAsync(query.Email) is not User user)
            return Errors.Authentication.InvalidCredentials;
        // password
        var isPassworValid = _passwordHasher.VerifyHashedPassword(query.Password, user.PasswordHash);

        if (!isPassworValid) return new[] { Errors.Authentication.InvalidCredentials };

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}