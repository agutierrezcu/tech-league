using Application.Abstractions.EventBus;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure.EventBus;

public sealed class EventBus(IServiceProvider serviceProvider) : IEventBus
{
    public async Task PublishAsync(IEnumerable<IDomainEvent> events, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(events);

        foreach (IDomainEvent @event in events)
        {
            foreach (IEventBusPublisher publisher in serviceProvider.GetServices<IEventBusPublisher>())
            {
                await publisher.TryPublishAsync(@event, ct);
            }
        }
    }
}
