using UrlShortener.Domain.UrlAggregate.Entity;

namespace UrlShortener.Application.Common.Interfaces.Repositories;

public interface IUrlRepository
{
    Task AddAsync(Url url, CancellationToken cancellationToken);
    Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken cancellationToken);
    Task<Url?> GetCodeAsync(string code, CancellationToken cancellationToken);
}