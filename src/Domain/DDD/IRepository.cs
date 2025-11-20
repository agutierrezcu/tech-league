using SharedKernel;

namespace Domain.DDD;

public interface IRepository
{
    Task SaveAsync(CancellationToken ct);
}

public interface IRepository<TAggregate, TRoot> : IRepository
    where TAggregate : AggregateRoot<TRoot>
{
    Task<Result<TAggregate>> GetAsync(CancellationToken ct);
}

public interface IRepository<TSpecification, TAggregate, TRoot> : IRepository
    where TAggregate : AggregateRoot<TRoot>
{
    Task<Result<TAggregate>> GetAsync(TSpecification specification, CancellationToken ct);
}
