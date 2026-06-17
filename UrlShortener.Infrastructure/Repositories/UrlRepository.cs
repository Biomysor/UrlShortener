using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.UrlAggregate.Entity;
using UrlShortener.Infrastructure.Persistance;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UrlRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Url url, CancellationToken cancellationToken)
    {
        await _dbContext.Urls.AddAsync(url, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Url url, CancellationToken cancellationToken)
    {
        _dbContext.Urls.Update(url);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        return await _dbContext.Urls.FirstOrDefaultAsync(x => x.LongUrl == longUrl, cancellationToken);
    }

    public async Task<Url?> GetCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await _dbContext.Urls.FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
}