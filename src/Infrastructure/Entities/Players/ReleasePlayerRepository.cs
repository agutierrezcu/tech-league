using Domain.Players.RRelease;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Entities.Players;

internal sealed class ReleasePlayerRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase
            <(ClubId ClubId, PlayerId PlayerId), ReleasePlayerAggregate, PlayerContract, ContractId>
                (dbContext)
{
    protected override async Task<Result<ReleasePlayerAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (ClubId ClubId, PlayerId PlayerId) specification,
            CancellationToken ct)
    {
        PlayerContract playerContract = await dbContext.Contracts
            .OfType<PlayerContract>()
            .FirstOrDefaultAsync(c =>
                c.PlayerId == specification.PlayerId && c.ClubId == specification.ClubId, ct);

        if (playerContract is null)
        {
            return Result.Failure<ReleasePlayerAggregate>(
                ReleasePlayerAggregateErrors.ContractNotFound);
        }

        return Result.Success(new ReleasePlayerAggregate(playerContract));
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        PlayerContract root, CancellationToken ct)
    {
        dbContext.Contracts.Remove(root);

        return Task.CompletedTask;
    }
}
