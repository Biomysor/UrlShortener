using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.UrlAggregate.Entity;
using UrlShortener.Domain.UserAggregate.Entity;

namespace UrlShortener.Infrastructure.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User>  Users { get; set; }

    public DbSet<Url>  Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}