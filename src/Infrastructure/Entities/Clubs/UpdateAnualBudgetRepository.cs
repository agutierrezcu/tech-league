using Domain.Clubs.UpdateBudget;
using Infrastructure.Database;
using SharedKernel;

using static Domain.Clubs.UpdateBudget.UpdateAnualBudgetAggregateErrors;

namespace Infrastructure.Entities.Clubs;

internal sealed class UpdateAnualBudgetRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<ClubId, UpdateAnualBudgetAggregate, Club>
            (dbContext)
{
    protected override async Task<Result<UpdateAnualBudgetAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, ClubId specification, CancellationToken ct)
    {
        ClubCommittedAnualBudget? clubInfo =
            await dbContext.Clubs.GetFinanceInfoByClubAsync(specification, ct);

        if (clubInfo is null)
        {
            return Result.Failure<UpdateAnualBudgetAggregate>(ClubNotFound);
        }

        UpdateAnualBudgetAggregate aggregator = new(clubInfo.Club, clubInfo.CommittedAnualBudget);

        return Result.Success(aggregator);
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext, Club root, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}
