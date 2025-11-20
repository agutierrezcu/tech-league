using Domain.DDD;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities;

internal abstract class ResultRootedRepositoryBase<TSpecification, TAggregate, TRoot>
    (TechLeagueDbContext dbContext)
        : IResultRepository<TSpecification, TAggregate, TRoot>
            where TAggregate : Aggregate, IRootedAggregate<TRoot>
{
    private IRootedAggregate<TRoot>? _rootedAggregate;

    public async Task<Result<TAggregate>> GetAsync(TSpecification specification, CancellationToken ct)
    {
        Result<TAggregate> aggregateResult = await CreateRootedAggregateAsync(dbContext, specification, ct);

        if (aggregateResult.IsFailure)
        {
            return aggregateResult;
        }

        _rootedAggregate = aggregateResult.Value;

        return Result.Success(aggregateResult.Value);
    }

    protected abstract Task<Result<TAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, TSpecification specification, CancellationToken ct);

    public async Task SaveAsync(CancellationToken ct)
    {
        if (_rootedAggregate is null)
        {
            throw new InvalidOperationException($"Rooted aggregate {typeof(TAggregate).Name} can not be null.");
        }

        await PerformOnRootEntityAsync(dbContext, _rootedAggregate.Root, ct);

        await dbContext.SaveChangesAsync(ct);
    }

    protected abstract Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        TRoot root, CancellationToken ct);
}
