using SharedKernel;

namespace Domain.DDD;

public interface IRepository
{
    Task SaveAsync(CancellationToken ct);
}

public interface IRepository<TAggregate, TRoot, TId> : IRepository
    where TAggregate : AggregateRoot<TRoot, TId>
    where TRoot : Entity<TId>
    where TId : notnull, IStronglyTyped<Guid>, new()
{
    Task<Result<TAggregate>> GetAsync(CancellationToken ct);
}

public interface IRepository<TSpecification, TAggregate, TRoot, TId> : IRepository
    where TAggregate : AggregateRoot<TRoot, TId>
    where TRoot : Entity<TId>
    where TId : notnull, IStronglyTyped<Guid>, new()
{
    Task<Result<TAggregate>> GetAsync(TSpecification specification, CancellationToken ct);
}
