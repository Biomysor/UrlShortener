using UrlShortener.Domain.Common;
using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Domain.UrlAggregate.Entity;

/// <summary>
///     Represents a shortened URL in the domain model.
///     Contains the original long URL, generated short code and creation date.
/// </summary>
public class Url : Entity<UrlId>
{
    /// <summary>
    ///     Required by Entity Framework Core.
    /// </summary>
    private Url(UrlId id, string longUrl, string code, DateTimeOffset createdAtUtc)
        : base(id)
    {
        LongUrl = longUrl;
        Code = code;
        CreatedAtUtc = createdAtUtc;
    }

    /// <summary>
    ///     Gets the original long URL provided by the user.
    /// </summary>
    public string LongUrl { get; private set; } = string.Empty;

    /// <summary>
    ///     Gets the generated short code used for redirection.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    ///     Gets the date and time when the URL was created.
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; private set; }

    /// <summary>
    ///     Sets the generated short code for this URL.
    /// </summary>
    /// <param name="code">Generated short code.</param>
    public void SetCode(string code)
    {
        Code = code;
    }

    /// <summary>
    ///     Creates a new URL entity with a unique identifier and empty code.
    ///     The short code is generated later by the application layer.
    /// </summary>
    /// <param name="longUrl">Original long URL.</param>
    /// <returns>New Url entity.</returns>
    public static Url Create(string longUrl)
    {
        return new Url(
            UrlId.CreateUnique(),
            longUrl,
            string.Empty,
            DateTimeOffset.UtcNow.DateTime);
    }
}