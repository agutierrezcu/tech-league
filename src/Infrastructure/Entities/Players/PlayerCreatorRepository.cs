using Domain.Players.Add;
using Infrastructure.Database;

namespace Infrastructure.Entities.Players;

internal sealed class PlayerCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<PlayerCreatorAggregate, Player>(dbContext)
{
    protected override Task<PlayerCreatorAggregate> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct)
    {
        return Task.FromResult(new PlayerCreatorAggregate());
    }

    protected override async Task PerformOnRootEntityAsync(
        TechLeagueDbContext dbContext, Player root, CancellationToken ct)
    {
        await dbContext.Players.AddAsync(root, ct);
    }
}
