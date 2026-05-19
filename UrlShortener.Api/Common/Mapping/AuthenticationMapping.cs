using Mapster;
using UrlShortener.Application.Authentication.Commands.Register;
using UrlShortener.Application.Authentication.Common;
using UrlShortener.Application.Authentication.Queries;
using UrlShortener.Contracts.Authentication;
using RegisterRequest = Microsoft.AspNetCore.Identity.Data.RegisterRequest;

namespace UrlShortener.Api.Common.Mapping;

public class AuthenticationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<LoginRequest, LoginQuery>();
        config.NewConfig<AuthenticationResult, AuthenticationResponce>()
            .Map(dest => dest.Id, src => src.user.Id.Value)
            .Map(dest => dest.Login, src => src.user.Login)
            .Map(dest => dest.Email, src => src.user.Email)
            .Map(dest => dest.Token, src => src.token);
    }
}