using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using TechLeagueDbContext dbContext =
            app.ApplicationServices.GetRequiredService<IDbContextFactory<TechLeagueDbContext>>()
                .CreateDbContext();

        dbContext.Database.Migrate();
    }
}
