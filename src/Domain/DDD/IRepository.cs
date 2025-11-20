using SharedKernel;

namespace Domain.DDD;

public interface IRepository
{
    Task SaveAsync(CancellationToken ct);
}

public interface IRepository<TSpecification, TAggregate, TRoot>
    : IRepository
        where TAggregate : Aggregate, IRootedAggregate<TRoot>
{
    Task<TAggregate> GetAsync(TSpecification specification, CancellationToken ct);
}

public interface IRepository<TAggregate, TRoot>
    : IRepository
        where TAggregate : Aggregate, IRootedAggregate<TRoot>
{
    Task<TAggregate> GetAsync(CancellationToken ct);
}

public interface IResultRepository<TSpecification, TAggregate, TRoot>
    : IRepository
        where TAggregate : Aggregate, IRootedAggregate<TRoot>
{
    Task<Result<TAggregate>> GetAsync(TSpecification specification, CancellationToken ct);
}
