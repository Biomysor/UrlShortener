
using UrlShortener.Application.Common.Interfaces.Services;

namespace UrlShortener.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}