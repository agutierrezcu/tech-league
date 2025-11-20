using SharedKernel;

namespace Infrastructure.EventBus;

public sealed class DefaultDomainEventBroadcaster<TDomainEvent>
    (IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> domainEventWrappers,
        IExecutionStrategy executionStrategy)
            : IDomainEventBroadcaster<TDomainEvent>
                where TDomainEvent : IDomainEvent
{
    public async ValueTask PublishAsync(TDomainEvent domainEvent, CancellationToken ct)
    {
        await executionStrategy.AsyncContinueOnException(
            domainEventWrappers, domainEvent, ct);
    }

    public ValueTask PublishAsync(IDomainEvent domainEvent, CancellationToken ct)
    {
        return PublishAsync((TDomainEvent)domainEvent, ct);
    }
}
