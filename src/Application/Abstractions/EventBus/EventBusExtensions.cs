using Domain.DDD;

namespace Application.Abstractions.EventBus;

public static class EventBusExtensions
{
    public static async Task PublishAsync(this IEventBus eventBus,
        Aggregate aggregate, CancellationToken ct)
    { 
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(aggregate);

        await eventBus.PublishAsync(aggregate.GetEvents(), ct);
    }
}

