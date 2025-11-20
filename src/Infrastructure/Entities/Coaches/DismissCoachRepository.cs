using Domain.Coaches.Dismiss;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using static Domain.Coaches.Dismiss.DismissCoachAggregateErrors;

namespace Infrastructure.Entities.Coaches;

internal sealed class DismissCoachRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase
            <(ClubId ClubId, CoachId CoachId), DismissCoachAggregate, CoachContract, ContractId>
                (dbContext)
{
    protected override async Task<Result<DismissCoachAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (ClubId ClubId, CoachId CoachId) specification,
            CancellationToken ct)
    {
        CoachContract coachContract = await dbContext.Contracts
            .OfType<CoachContract>()
            .FirstOrDefaultAsync(c =>
                c.CoachId == specification.CoachId && c.ClubId == specification.ClubId, ct);

        if (coachContract is null)
        {
            return Result.Failure<DismissCoachAggregate>(ContractNotFound);
        }

        return Result.Success(new DismissCoachAggregate(coachContract));
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        CoachContract root, CancellationToken ct)
    {
        dbContext.Contracts.Remove(root);

        return Task.CompletedTask;
    }
}
