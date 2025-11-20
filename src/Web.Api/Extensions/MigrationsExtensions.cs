using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Extensions;

internal static class MigrationsExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        IDbContextFactory<TechLeagueDbContext> dbContextFactory =
            app.ApplicationServices.GetRequiredService<IDbContextFactory<TechLeagueDbContext>>();

        using TechLeagueDbContext dbContext = dbContextFactory.CreateDbContext();

        dbContext.Database.Migrate();
    }
}
