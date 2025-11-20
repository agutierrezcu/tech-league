using SharedKernel;

namespace Infrastructure.EventBus;

public interface IEventBusPublisher
{
    ValueTask<bool> TryPublishAsync(IDomainEvent @event, CancellationToken ct);
}
