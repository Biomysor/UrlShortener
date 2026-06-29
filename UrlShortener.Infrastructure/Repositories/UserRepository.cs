using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.UserAggregate.Entity;
using UrlShortener.Infrastructure.Persistance;

namespace UrlShortener.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async  Task<User?> GetByEmailAsync(string email,  CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email,  cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await  dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}