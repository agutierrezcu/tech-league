using SharedKernel;
using StronglyTypedIds;

namespace Domain.DDD;

[StronglyTypedId]
public readonly partial struct AggregateId
{
}

public abstract class AggregateRoot<T> : IRootedAggregate<T>
{
    public AggregateId Id => AggregateId.New();

    protected abstract T Root { get; }

    T IRootedAggregate<T>.Root => Root;

    private readonly Queue<Func<IDomainEvent>> _eventsHolder = new();

    protected void RegisterEvent(Func<IDomainEvent> eventFactory)
        => _eventsHolder.Enqueue(eventFactory);

    public IEnumerable<IDomainEvent> GetEvents()
    {
        while (_eventsHolder.TryDequeue(out Func<IDomainEvent>? eventFactory))
        {
            yield return eventFactory();
        }
    }
}
