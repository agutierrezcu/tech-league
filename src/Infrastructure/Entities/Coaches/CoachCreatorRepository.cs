using Domain.Coaches.Add;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities.Coaches;

internal sealed class CoachCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<CoachCreatorAggregate, Coach, CoachId>
            (dbContext)
{
    protected override Task<Result<CoachCreatorAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct)
    {
        Coach root = new()
        {
            FullName = string.Empty,
            Experience = int.MinValue
        };

        CoachCreatorAggregate aggregate = new(root);

        return Task.FromResult(Result.Success(aggregate));
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext, Coach root,
        CancellationToken ct)
    {
        return dbContext.Coaches.AddAsync(root, ct).AsTask();
    }
}
