using Domain.Coaches.Hire;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using static Domain.Coaches.Hire.HireCoachAggregateErrors;

namespace Infrastructure.Entities.Coaches;

internal sealed class HireCoachPlayerRepository
    (TechLeagueDbContext dbContext)
        : ResultRootedRepositoryBase
            <(ClubId ClubId, CoachId CoachId), HireCoachAggregate, HireCoachAggregateRoot>
                (dbContext)
{
    protected override async Task<Result<HireCoachAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (ClubId ClubId, CoachId CoachId) specification, 
            CancellationToken ct)
    {
        Coach? coach = await dbContext.Coaches
            .Include(c => c.CurrentContract)
            .FirstOrDefaultAsync(c => c.Id == specification.CoachId, ct);

        if (coach is null)
        {
            return Result.Failure<HireCoachAggregate>(CoachNotFound);
        }

        ClubCommittedAnualBudget? clubInfo = await dbContext.Clubs.GetFinanceInfoByClubAsync(specification.ClubId, ct);

        if (clubInfo is null)
        {
            return Result.Failure<HireCoachAggregate>(ClubNotFound);
        }

        HireCoachAggregateRoot root = new(clubInfo.Club, coach);

        return Result.Success(new HireCoachAggregate(root, clubInfo.CommittedAnualBudget));
    }

    protected override async Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
       HireCoachAggregateRoot root, CancellationToken ct)
    {
        await dbContext.Contracts.AddAsync(root.CoachContract, ct);
    }
}
