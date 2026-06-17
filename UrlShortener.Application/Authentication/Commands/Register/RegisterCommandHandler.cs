using ErrorOr;
using MediatR;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Authentication.Commands.Register;

/// <summary>
///     Handles user registration commands.
///     Checks email uniqueness, hashes the password, saves the user and generates a JWT token.
/// </summary>
public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    /// <summary>
    ///     Processes a user registration request.
    /// </summary>
    /// <param name="command">Registration command containing login, email and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    ///     Authentication result with user data and JWT token,
    ///     or DuplicateEmail error if user already exists.
    /// </returns>
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command,
        CancellationToken cancellationToken)
    {
        if (await userRepository.GetByEmailAsync(command.Email, cancellationToken) is not null)
            return Errors.User.DuplicateEmail(command.Email);
        
        var passwordHash = _passwordHasher.HashPassword(command.Password);

        var user = User.Create(command.Login, command.Email, passwordHash);

        await userRepository.AddAsync(user,  cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}