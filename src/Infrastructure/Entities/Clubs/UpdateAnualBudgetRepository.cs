using Domain.Clubs.UpdateBudget;
using Domain.DDD;
using Infrastructure.Database;
using SharedKernel;

using static Domain.Clubs.UpdateBudget.UpdateAnualBudgetAggregateErrors;

namespace Infrastructure.Entities.Clubs;

internal sealed class UpdateAnualBudgetRepository
    (TechLeagueDbContext dbContext)
        : IResultRepository<ClubId, UpdateAnualBudgetAggregate, Club>
{
    public async Task<Result<UpdateAnualBudgetAggregate>> GetAsync(ClubId clubId,
        CancellationToken ct)
    {
        ClubCommittedAnualBudget? clubInfo = await dbContext.Clubs.GetFinanceInfoByClubAsync(clubId, ct);

        if (clubInfo is null)
        {
            return Result.Failure<UpdateAnualBudgetAggregate>(ClubNotFound);
        }

        UpdateAnualBudgetAggregate aggregator = new(clubInfo.Club, clubInfo.CommittedAnualBudget);

        return Result.Success(aggregator);
    }

    public Task SaveAsync(CancellationToken ct) 
        => dbContext.SaveChangesAsync(ct);
}
