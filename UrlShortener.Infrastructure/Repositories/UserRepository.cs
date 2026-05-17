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

    public async  Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await  _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}