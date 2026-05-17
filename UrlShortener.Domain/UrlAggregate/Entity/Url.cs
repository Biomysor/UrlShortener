using UrlShortener.Domain.Common;
using UrlShortener.Domain.UrlAggregate.ValueObjects;


namespace UrlShortener.Domain.UrlAggregate.Entity;

public class Url : Entity<UrlId>
{
    public string LongUrl { get; private set; } = string.Empty;
    public string Code{ get; private set; }  = string.Empty;
    public DateTime CreatedAt { get; set; }

    private Url(UrlId id, string longUrl, string code, DateTime createdAt)
        : base(id)
    {
        LongUrl = longUrl;
        Code = code;
        CreatedAt = createdAt;
    }

    public void SetCode(string code)
    {
        Code = code;
    }

    public static Url Create(string longUrl)
    {
        return new Url(
            UrlId.CreateUnique(),
            longUrl,
            string.Empty,
            DateTime.UtcNow);
    }
}