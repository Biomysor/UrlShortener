namespace UrlShortener.Domain.UserAggregate;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}