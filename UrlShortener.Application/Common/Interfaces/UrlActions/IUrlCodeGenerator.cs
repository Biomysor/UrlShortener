using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Application.Common.Interfaces.UrlActions;

/// <summary>
/// Provides functionality for generating short URL codes.
/// </summary>
public interface IUrlCodeGenerator
{
    /// <summary>
    /// Generates a short code based on URL identifier.
    /// </summary>
    /// <param name="id">URL identifier.</param>
    /// <returns>Generated short code.</returns>
    string GenerateCode(UrlId id);
}