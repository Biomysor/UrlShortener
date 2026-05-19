using Microsoft.AspNetCore.Http;
using UrlShortener.Application.Common.Interfaces.UrlActions;

namespace UrlShortener.Infrastructure.UrlActions;

public class ShortUrlBuilder(IHttpContextAccessor httpContextAccessor) : IShortUrlBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string BuildShortUrl(string code)
    {
        var request = _httpContextAccessor.HttpContext!.Request;
        return $"{request.Scheme}://{request.Host}/{code}";
    }
}