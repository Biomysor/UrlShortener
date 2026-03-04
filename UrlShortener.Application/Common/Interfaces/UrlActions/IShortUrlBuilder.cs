namespace UrlShortener.Application.Common.Interfaces.UrlActions;

public interface IShortUrlBuilder
{
    string BuildShortUrl(string code);
}