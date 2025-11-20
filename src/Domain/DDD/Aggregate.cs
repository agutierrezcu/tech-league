using SharedKernel;
using StronglyTypedIds;

namespace Domain.DDD;

[StronglyTypedId]
public readonly partial struct AggregateId
{
}

public abstract class Aggregate
{
    public AggregateId Id => new(Guid.CreateVersion7());

    private readonly Queue<Func<IDomainEvent>> _eventsHolder = new();

    protected void RegisterEvent(Func<IDomainEvent> eventFactory)
        => _eventsHolder.Enqueue(eventFactory);

    public IEnumerable<IDomainEvent> GetEvents()
    {
        HashSet<IDomainEvent> events = [];

        while (_eventsHolder.TryDequeue(out Func<IDomainEvent>? eventFactory))
        {
            events.Add(eventFactory());
        }

        return events;
    }
}

public abstract class Aggregate<T> : Aggregate, IRootedAggregate<T>
{
    protected abstract T Root { get; }

    T IRootedAggregate<T>.Root => Root;
}
