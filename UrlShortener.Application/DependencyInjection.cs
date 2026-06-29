using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UrlShortener.Application.Authentication.Behavior;
using UrlShortener.Application.Authentication.Commands.Register;
using UrlShortener.Application.Authentication.Queries;
using UrlShortener.Application.UrlActions.Commands;
using UrlShortener.Application.UrlActions.Queries.UrlQueries;

namespace UrlShortener.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.LicenseKey = configuration["MediatR:LicenseKey"];
        });


        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidation>();
        services.AddScoped<IValidator<LoginQuery>, LoginQueryValidation>();
        services.AddScoped<IValidator<ShortenUrlCommand>, ShortenUrlValidation>();
        services.AddScoped<IValidator<UrlQuery>, UrlQueryValidation>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}