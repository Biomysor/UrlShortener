using MediatR;
using ErrorOr;
using MapsterMapper;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private  readonly IPasswordHasher _passwordHasher = passwordHasher;
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if ( await userRepository.GetByEmailAsync(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail(command.Email);
        }
        var passwordHash = _passwordHasher.HashPassword(command.Password);
        
        var user = User.Create(command.Login, command.Email, passwordHash);

        await userRepository.AddAsync(user);
        
        var token = jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(user, token);

    }
}