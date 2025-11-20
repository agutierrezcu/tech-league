using Domain.DDD;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Entities;

internal abstract class RootedRepositoryBase<TSpecification, TAggregate, TRoot, TId>
    (TechLeagueDbContext dbContext)
        : IRepository<TSpecification, TAggregate, TRoot, TId>
            where TAggregate : AggregateRoot<TRoot, TId>
            where TRoot : Entity<TId>
            where TId : notnull, IStronglyTyped<Guid>, new()
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

    protected virtual Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        TRoot root, CancellationToken ct)
    { 
        return Task.CompletedTask;
    }
}

internal abstract class RootedRepositoryBase<TAggregate, TRoot, TId>
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase<object, TAggregate, TRoot, TId>(dbContext),
            IRepository<TAggregate, TRoot, TId>
                where TAggregate : AggregateRoot<TRoot, TId>
                where TRoot : Entity<TId>
                where TId : notnull, IStronglyTyped<Guid>, new()
{
    public Task<Result<TAggregate>> GetAsync(CancellationToken ct)
    {
        return GetAsync(new(), ct);
    }

    protected override Task<Result<TAggregate>> CreateRootedAggregateAsync(TechLeagueDbContext dbContext,
        object _, CancellationToken ct)
    {
        return CreateRootedAggregateAsync(dbContext, ct);
    }

    protected abstract Task<Result<TAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, CancellationToken ct);
}
