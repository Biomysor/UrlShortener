using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.UrlAggregate.Entity;
using UrlShortener.Infrastructure.Persistance;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository(ApplicationDbContext dbContext) : IUrlRepository
{
    public async Task AddAsync(Url url, CancellationToken cancellationToken)
    {
        await dbContext.Urls.AddAsync(url, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        return await dbContext.Urls.FirstOrDefaultAsync(x => x.LongUrl == longUrl, cancellationToken);
    }

    public async Task<Url?> GetCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await dbContext.Urls.FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
}