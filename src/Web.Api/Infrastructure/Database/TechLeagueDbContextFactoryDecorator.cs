using Microsoft.EntityFrameworkCore;

namespace Web.Api.Infrastructure.Database;


public class TechLeagueDbContextFactoryDecorator<TDbContext>
    (IDbContextFactory<TDbContext> inner)
        : IDbContextFactory<TDbContext>
            where TDbContext : DbContext
{
    public TDbContext CreateDbContext()
        => inner.CreateDbContext();
}
