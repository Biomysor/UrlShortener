using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Application.Common.Interfaces.Authentication;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Application.Common.Interfaces.Services;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Infrastructure.Authentication;
using UrlShortener.Infrastructure.Persistance;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Services;
using UrlShortener.Infrastructure.UrlActions;

namespace UrlShortener.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddAuth(configuration)
            .AddPersistance(configuration);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        var rabbitMqOptions = new RabbitMqOptions();
        configuration.Bind(RabbitMqOptions.SectionName, rabbitMqOptions);
        services.AddSingleton(Options.Create(rabbitMqOptions));

        services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, "/", h =>
                {
                    h.Username(rabbitMqOptions.Username!);
                    h.Password(rabbitMqOptions.Password!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<ICacheService, CachingService>();
        services.AddHttpContextAccessor();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUrlCodeGenerator, UrlCodeGenerator>();
        services.AddScoped<IShortUrlBuilder, ShortUrlBuilder>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }

    private static IServiceCollection AddPersistance(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUrlRepository, UrlRepository>();

        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Secret))
                };
            });

        return services;
    }
}