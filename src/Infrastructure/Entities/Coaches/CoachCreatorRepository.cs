using Domain.Coaches.Add;
using Infrastructure.Database;

namespace Infrastructure.Entities.Coaches;

internal sealed class CoachCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<CoachCreatorAggregate, Coach>(dbContext)
{
    protected override Task<CoachCreatorAggregate> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct)
    {
        return Task.FromResult(new CoachCreatorAggregate());
    }

    protected override async Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext, 
        Coach root, CancellationToken ct)
    {
        await dbContext.Coaches.AddAsync(root, ct);
    }
}
