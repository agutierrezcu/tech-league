using Domain.DDD;
using SharedKernel;

namespace Application.Abstractions.EventBus;

public static class EventBusExtensions
{
    public static async Task PublishAsync<TRoot, TId>(this IEventBus eventBus,
        AggregateRoot<TRoot, TId> aggregate, CancellationToken ct)
            where TRoot : Entity<TId>
            where TId : notnull, IStronglyTyped<Guid>, new()
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(aggregate);

        await eventBus.PublishAsync(aggregate.GetEvents(), ct);
    }
}

