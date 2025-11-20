using Application.Clubs.Projections;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Infrastructure.Background;

public sealed class ClubFinanceStatusProjectionInitializerBackgroundService
    (IServiceScopeFactory serviceScopeFactory)
        : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        ClubId[] clubIds = EFCoreDataSeeder.ClubIdGenerator.UsedIds;

        if (!clubIds.Any())
        {
            return;
        }

        await using AsyncServiceScope asyncScope = serviceScopeFactory.CreateAsyncScope();

        using TechLeagueDbContext techLeagueDbContext = 
            asyncScope.ServiceProvider.GetRequiredService<TechLeagueDbContext>();

        foreach (ClubId clubId in clubIds)
        {
            ClubFinanceStatusProjection? projection = await techLeagueDbContext.Clubs
                .Where(c => c.Id == clubId)
                .Select(c => new ClubFinanceStatusProjection
                {
                    ClubId = c.Id,
                    AnualBudget = c.AnualBudget,
                    CommittedInPlayers = c.Contracts.OfType<PlayerContract>().Sum(s => s.AnualSalary),
                    PlayerContractCount = c.Contracts.OfType<PlayerContract>().Count(),
                    CommittedInCoaches = c.Contracts.OfType<CoachContract>().Sum(s => s.AnualSalary),
                    CoachContractCount = c.Contracts.OfType<CoachContract>().Count(),
                })
                .FirstOrDefaultAsync(ct);

            await techLeagueDbContext.FinanceStatusProjection.AddAsync(projection!, ct);
        }

        await techLeagueDbContext.SaveChangesAsync(ct);
    }
}
