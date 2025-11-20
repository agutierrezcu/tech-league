using Domain.Players.Add;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities.Players;

internal sealed class PlayerCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<PlayerCreatorAggregate, Player, PlayerId>
            (dbContext)
{
    protected override Task<Result<PlayerCreatorAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct)
    {
        Player root = new()
        {
            FullName = string.Empty
        };

        PlayerCreatorAggregate aggregate = new(root);

        return Task.FromResult(Result.Success(aggregate));
    }

    protected override Task PerformOnRootEntityAsync(
        TechLeagueDbContext dbContext, Player root, CancellationToken ct)
    {
        return dbContext.Players.AddAsync(root, ct).AsTask();
    }
}
