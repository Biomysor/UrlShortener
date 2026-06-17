namespace UrlShortener.Infrastructure.Services;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string? Host { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}