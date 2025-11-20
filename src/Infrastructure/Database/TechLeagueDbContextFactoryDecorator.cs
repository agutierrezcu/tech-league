using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;


public class TechLeagueDbContextFactoryDecorator<TDbContext>(IDbContextFactory<TDbContext> inner)
    : IDbContextFactory<TDbContext>
        where TDbContext : DbContext
{
    public TDbContext CreateDbContext()
    {
        return inner.CreateDbContext();
    }
}
