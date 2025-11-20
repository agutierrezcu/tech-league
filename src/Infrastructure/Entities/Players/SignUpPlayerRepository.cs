using Domain.Players.SignUp;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using Errors = Domain.Players.SignUp.SignUpPlayerAggregateErrors;

namespace Infrastructure.Entities.Players;

internal sealed class SignUpPlayerRepository
    (TechLeagueDbContext dbContext, IGetClubCommittedAnualBudgetQueryable clubCommittedAnualBudgetQueryable)
        : RootedRepositoryBase
            <(ClubId ClubId, PlayerId PlayerId), SignUpPlayerAggregate, Player, PlayerId>
                (dbContext)
{
    protected override async Task<Result<SignUpPlayerAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (ClubId ClubId, PlayerId PlayerId) specification,
            CancellationToken ct)
    {
        Task<Player?> playerTask = dbContext.Players
            .Include(c => c.CurrentContract)
            .FirstOrDefaultAsync(c => c.Id == specification.PlayerId, ct);

        Task<ClubCommittedAnualBudgetInfo?> clubCommittedAnualBudgetTask =
           clubCommittedAnualBudgetQueryable.GetAsync(specification.ClubId, ct);

        await Task.WhenAll(playerTask, clubCommittedAnualBudgetTask);

        Player? player = await playerTask;

        if (player is null)
        {
            return Result.Failure<SignUpPlayerAggregate>(Errors.PlayerNotFound);
        }

        ClubCommittedAnualBudgetInfo? clubInfo = await clubCommittedAnualBudgetTask;

        if (clubInfo is null)
        {
            return Result.Failure<SignUpPlayerAggregate>(Errors.ClubNotFound);
        }

        Club club = clubInfo.Club;

        SignUpPlayerAggregate aggregate = new(player,
            (club.Id, club.AnualBudget, clubInfo.CommittedAnualBudget));

        return Result.Success(aggregate);
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext, Player root,
        CancellationToken ct)
    {
        return dbContext.Contracts.AddAsync(root.CurrentContract!, ct).AsTask();
    }
}
