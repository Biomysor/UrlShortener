namespace UrlShortener.Messaging.Contracts.Events;

/// <summary>
///     Integration event that is published when a short URL is redirected.
///     Used by analytics service to update redirect statistics.
/// </summary>
/// <param name="UrlId">Identifier of the URL.</param>
/// <param name="Code">Short URL code.</param>
/// <param name="LongUrl">Original long URL.</param>
/// <param name="RedirectedAtUtc">Date and time of redirect.</param>
/// <param name="IpAddress">Optional client IP address.</param>
/// <param name="UserAgent">Optional client user agent.</param>
public record UrlRedirectedEvent(
    Guid UrlId,
    string Code,
    string LongUrl,
    DateTimeOffset RedirectedAtUtc,
    string? IpAddress,
    string? UserAgent);