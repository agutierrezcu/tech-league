using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using TechLeagueDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<TechLeagueDbContext>();

        dbContext.Database.Migrate();
    }
}
