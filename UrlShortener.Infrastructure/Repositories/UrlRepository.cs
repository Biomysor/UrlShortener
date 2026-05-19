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

    public async Task AddAsync(Url url)
    {
        await _dbContext.Urls.AddAsync(url);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Url url)
    {
        _dbContext.Urls.Update(url);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Url?> GetByLongUrlAsync(string longUrl)
    {
        return await _dbContext.Urls.FirstOrDefaultAsync(x => x.LongUrl == longUrl);
    }

    public async Task<Url?> GetCodeAsync(string code)
    {
        return await _dbContext.Urls.FirstOrDefaultAsync(x => x.Code == code);
    }
}