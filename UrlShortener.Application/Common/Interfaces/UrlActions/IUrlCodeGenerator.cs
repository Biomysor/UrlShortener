using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Application.Common.Interfaces.UrlActions;

public interface IUrlCodeGenerator
{
    string GenerateCode(UrlId id);
}