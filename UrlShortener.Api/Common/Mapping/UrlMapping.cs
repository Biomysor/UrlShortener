using Mapster;
using UrlShortener.Application.Authentication.Queries;
using UrlShortener.Application.UrlActions.Commands;
using UrlShortener.Application.UrlActions.Common;
using UrlShortener.Application.UrlActions.Queries;
using UrlShortener.Application.UrlActions.Queries.UrlQueries;
using UrlShortener.Contracts.UrlAction;

namespace UrlShortener.Api.Common.Mapping;

public class UrlMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UrlRequest ,ShortenUrlCommand>();
        config.NewConfig<UrlRequest, UrlQuery>();
        config.NewConfig<UrlResult, UrlResponse>();
    }
}