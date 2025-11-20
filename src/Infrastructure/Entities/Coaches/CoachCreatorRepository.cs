using Domain.Coaches.Add;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities.Coaches;

internal sealed class CoachCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<CoachCreatorAggregate, Coach>
            (dbContext)
{
    protected override Task<Result<CoachCreatorAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct)
    {
        Coach root = new()
        {
            FullName = string.Empty,
            Experience = 0
        };

        return Task.FromResult(Result.Success(new CoachCreatorAggregate(root)));
    }

    protected override async Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext, Coach root,
        CancellationToken ct)
    {
        await dbContext.Coaches.AddAsync(root, ct);
    }
}
