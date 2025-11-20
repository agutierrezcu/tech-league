using Domain.Players.SignUp;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using static Domain.Players.SignUp.SignUpPlayerAggregateErrors;

namespace Infrastructure.Entities.Players;

internal sealed class SignUpPlayerRepository
    (TechLeagueDbContext dbContext)
        : ResultRootedRepositoryBase
            <(ClubId ClubId, PlayerId PlayerId), SignUpPlayerAggregate, SignUpPlayerAggregateRoot>
                (dbContext)
{
    protected override async Task<Result<SignUpPlayerAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (ClubId ClubId, PlayerId PlayerId) specification, CancellationToken ct)
    {
        Player? player = await dbContext.Players
            .Include(c => c.CurrentContract)
            .FirstOrDefaultAsync(c => c.Id == specification.PlayerId, ct);

        if (player is null)
        {
            return Result.Failure<SignUpPlayerAggregate>(PlayerNotFound);
        }

        ClubCommittedAnualBudget? clubInfo = 
            await dbContext.Clubs.GetFinanceInfoByClubAsync(specification.ClubId, ct);

        if (clubInfo is null)
        {
            return Result.Failure<SignUpPlayerAggregate>(ClubNotFound);
        }

        SignUpPlayerAggregateRoot root = new(clubInfo.Club, player);

        return Result.Success(new SignUpPlayerAggregate(root, clubInfo.CommittedAnualBudget));
    }

    protected override async Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        SignUpPlayerAggregateRoot root, CancellationToken ct)
    {
        await dbContext.Contracts.AddAsync(root.PlayerContract, ct);
    }
}
