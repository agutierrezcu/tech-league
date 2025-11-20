using Domain.Coaches.Hire;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using Errors = Domain.Coaches.Hire.HireCoachAggregateErrors;

namespace Infrastructure.Entities.Coaches;

internal sealed class HireCoachRepository
    (TechLeagueDbContext dbContext, IGetClubCommittedAnualBudgetQueryable clubCommittedAnualBudgetQueryable)
        : RootedRepositoryBase
            <(ClubId ClubId, CoachId CoachId), HireCoachAggregate, Coach, CoachId>
                (dbContext)
{
    protected override async Task<Result<HireCoachAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (ClubId ClubId, CoachId CoachId) specification,
            CancellationToken ct)
    {
        Task<Coach?> coachTask = dbContext.Coaches
            .Include(c => c.CurrentContract)
            .FirstOrDefaultAsync(c => c.Id == specification.CoachId, ct);

        Task<ClubCommittedAnualBudgetInfo?> clubCommittedAnualBudgetTask =
            clubCommittedAnualBudgetQueryable.GetAsync(specification.ClubId, ct);

        await Task.WhenAll(coachTask, clubCommittedAnualBudgetTask);

        Coach? coach = await coachTask;

        if (coach is null)
        {
            return Result.Failure<HireCoachAggregate>(Errors.CoachNotFound);
        }

        ClubCommittedAnualBudgetInfo? clubInfo = await clubCommittedAnualBudgetTask;

        if (clubInfo is null)
        {
            return Result.Failure<HireCoachAggregate>(Errors.ClubNotFound);
        }

        Club club = clubInfo.Club;

        HireCoachAggregate aggregate = new(coach, 
            (club.Id, club.AnualBudget, clubInfo.CommittedAnualBudget));

        return Result.Success(aggregate);
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext, Coach root, 
        CancellationToken ct)
    {
        return dbContext.Contracts.AddAsync(root.CurrentContract!, ct).AsTask();
    }
}
