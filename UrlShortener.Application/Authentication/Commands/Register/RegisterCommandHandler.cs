using MediatR;
using ErrorOr;
using MapsterMapper;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.Common.Errors;
using UrlShortener.Domain.UserAggregate;

namespace UrlShortener.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (userRepository.GetByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail(command.Email);
        }
        
        var user = new User
        {
            Login = command.Login,
            Email = command.Email,
            Password = command.Password
        };
        
        userRepository.Add(user);
        
        var token = jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(user, token);

    }
}