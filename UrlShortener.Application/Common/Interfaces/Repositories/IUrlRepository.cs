using UrlShortener.Domain.UrlAggregate;
using UrlShortener.Domain.UrlAggregate.Entity;

namespace UrlShortener.Application.Common.Interfaces.Repositories;

public interface IUrlRepository
{
    Task AddAsync(Url url);
    Task UpdateAsync(Url url);
    Task<Url?> GetByLongUrlAsync(string longUrl);
    Task<Url?> GetCodeAsync(string code);
}