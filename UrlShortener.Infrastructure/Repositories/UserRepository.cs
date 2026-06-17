using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.UserAggregate;
using UrlShortener.Domain.UserAggregate.Entity;
using UrlShortener.Infrastructure.Persistance;

namespace UrlShortener.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async  Task<User?> GetByEmailAsync(string email,  CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email,  cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await  _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}