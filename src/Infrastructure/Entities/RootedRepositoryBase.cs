using Domain.DDD;
using Infrastructure.Database;

namespace Infrastructure.Entities;

internal abstract class RootedRepositoryBase<TAggregate, TRoot>
    (TechLeagueDbContext dbContext)
        : IRepository<TAggregate, TRoot>
            where TAggregate : Aggregate, IRootedAggregate<TRoot>
{
    private IRootedAggregate<TRoot>? _rootedAggregate;

    public async Task<TAggregate> GetAsync(CancellationToken ct)
    {
        TAggregate aggregate = await CreateRootedAggregateAsync(dbContext, ct);

        _rootedAggregate = aggregate;

        return aggregate;
    }

    protected abstract Task<TAggregate> CreateRootedAggregateAsync(TechLeagueDbContext dbContext, 
        CancellationToken ct);

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
