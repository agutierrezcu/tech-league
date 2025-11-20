using Domain.Clubs.UpdateBudget;
using Infrastructure.Database;
using SharedKernel;

using static Domain.Clubs.UpdateBudget.UpdateAnualBudgetAggregateErrors;

namespace Infrastructure.Entities.Clubs;

internal sealed class UpdateAnualBudgetRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<ClubId, UpdateAnualBudgetAggregate, Club, ClubId>
            (dbContext)
{
    protected override async Task<Result<UpdateAnualBudgetAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, ClubId specification, CancellationToken ct)
    {
        ClubCommittedAnualBudgetInfo? clubInfo =
            await dbContext.Clubs.GetFinanceInfoByClubAsync(specification, ct);

        if (clubInfo is null)
        {
            return Result.Failure<UpdateAnualBudgetAggregate>(ClubNotFound);
        }

        UpdateAnualBudgetAggregate aggregate = new(clubInfo.Club, clubInfo.CommittedAnualBudget);

        return Result.Success(aggregate);
    }
}
