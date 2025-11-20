using Application.Clubs.Projections;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Infrastructure.Background;

public sealed class ClubFinanceStatusProjectionInitializerBackgroundService
    (IDbContextFactory<TechLeagueDbContext> dbContextFactory)
        : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<ClubId> usedClubsId = EfCoreDataSeeder.UsedClubsId;

        if (!usedClubsId.Any())
        {
            return;
        }

        using TechLeagueDbContext dbContext =
            await dbContextFactory.CreateDbContextAsync(stoppingToken);

        ClubFinanceStatusProjection[] projections = await dbContext.Clubs
            .Where(c => usedClubsId.Contains(c.Id))
            .Select(c => new ClubFinanceStatusProjection
            {
                ClubId = c.Id,
                AnualBudget = c.AnualBudget,
                CommittedInPlayers = c.Contracts.OfType<PlayerContract>().Sum(s => s.AnualSalary),
                PlayerContractCount = c.Contracts.OfType<PlayerContract>().Count(),
                CommittedInCoaches = c.Contracts.OfType<CoachContract>().Sum(s => s.AnualSalary),
                CoachContractCount = c.Contracts.OfType<CoachContract>().Count()
            })
            .ToArrayAsync(stoppingToken);

        await dbContext.BulkInsertAsync(projections, stoppingToken);
    }
}
