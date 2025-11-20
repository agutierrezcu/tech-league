using Domain.Players.Add;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities.Players;

internal sealed class PlayerCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<PlayerCreatorAggregate, Player>
            (dbContext)
{
    protected override Task<Result<PlayerCreatorAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct)
    {
        Player root = new()
        {
            FullName = string.Empty
        };

        return Task.FromResult(Result.Success(new PlayerCreatorAggregate(root)));
    }

    protected override async Task PerformOnRootEntityAsync(
        TechLeagueDbContext dbContext, Player root, CancellationToken ct)
    {
        await dbContext.Players.AddAsync(root, ct);
    }
}
