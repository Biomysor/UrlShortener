using UrlShortener.Domain.UrlAggregate;

namespace UrlShortener.Application.Common.Interfaces.Repositories;

public interface IUrlRepository
{
    void Add(Url url);
    void Update(Url url);
    Task<Url?> GetByLongUrlAsync(string longUrl);
    Task<Url?> GetCodeAsync(string code);
}