namespace UrlShortener.Application.Common.Interfaces.UrlActions;

/// <summary>
/// Builds a full short URL from a generated short code.
/// </summary>
public interface IShortUrlBuilder
{
    /// <summary>
    /// Builds a complete short URL using current HTTP request scheme and host.
    /// </summary>
    /// <param name="code">Short URL code.</param>
    /// <returns>Full short URL.</returns>
    string BuildShortUrl(string code);
}