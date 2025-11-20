using Domain.DDD;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities;

internal abstract class RootedRepositoryBase<TSpecification, TAggregate, TRoot>
    (TechLeagueDbContext dbContext)
        : IRepository<TSpecification, TAggregate, TRoot>
            where TAggregate : AggregateRoot<TRoot>
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

    public async Task SaveAsync(CancellationToken ct)
    {
        if (_rootedAggregate is null)
        {
            throw new InvalidOperationException($"Rooted aggregate {typeof(TAggregate).Name} can not be null.");
        }

        await PerformOnRootEntityAsync(dbContext, _rootedAggregate.Root, ct);

        await dbContext.SaveChangesAsync(ct);
    }

    protected abstract Task<Result<TAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, TSpecification specification, CancellationToken ct);

    protected abstract Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        TRoot root, CancellationToken ct);
}

internal abstract class RootedRepositoryBase<TAggregate, TRoot>
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<object, TAggregate, TRoot>(dbContext),
            IRepository<TAggregate, TRoot>
                where TAggregate : AggregateRoot<TRoot>
{
    public Task<Result<TAggregate>> GetAsync(CancellationToken ct)
        => GetAsync(new(), ct);

    protected override Task<Result<TAggregate>> CreateRootedAggregateAsync(TechLeagueDbContext dbContext,
        object _, CancellationToken ct)
            => CreateRootedAggregateAsync(dbContext, ct);

    protected abstract Task<Result<TAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct);
}
