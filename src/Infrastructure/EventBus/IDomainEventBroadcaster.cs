using SharedKernel;

namespace Infrastructure.EventBus;

public interface IDomainEventBroadcaster
{
    ValueTask PublishAsync(IDomainEvent domainEvent, CancellationToken ct);
}

public interface IDomainEventBroadcaster<in TDomainEvent>
    : IDomainEventBroadcaster
        where TDomainEvent : IDomainEvent
{
    ValueTask PublishAsync(TDomainEvent domainEvent, CancellationToken ct);
}
