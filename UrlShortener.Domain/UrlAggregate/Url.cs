namespace UrlShortener.Domain.UrlAggregate;

public class Url
{
    public long Id { get; private set; }        
    public Guid PublicId { get; private set; }
    public string LongUrl { get; private set; } = string.Empty;
    public string Code{ get; private set; }  = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    private Url() { } 

    public Url(string longUrl)
    {
        LongUrl = longUrl;
        CreatedAt = DateTime.UtcNow;
        PublicId = Guid.NewGuid();
    }

    public void SetCode(string code)
    {
        Code = code;
    }
}