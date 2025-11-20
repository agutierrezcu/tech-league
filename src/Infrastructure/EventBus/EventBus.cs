using Application.Abstractions.EventBus;
using SharedKernel;

namespace Infrastructure.EventBus;

public sealed class EventBus(IEnumerable<IEventBusPublisher> publishers) : IEventBus
{
    public async Task PublishAsync(IEnumerable<IDomainEvent> events, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(events);

        foreach (IDomainEvent @event in events)
        {
            foreach (IEventBusPublisher publisher in publishers)
            {
                await publisher.TryPublishAsync(@event, ct);
            }
        }
    }
}
