using SharedKernel;

namespace Web.Api.Infrastructure.Background;

public sealed class DefaultDomainEventBroadcaster<TDomainEvent>
    (IServiceScopeFactory serviceScopeFactory)
        : IDomainEventBroadcaster<TDomainEvent>
            where TDomainEvent : IDomainEvent
{
    public async ValueTask PublishAsync(TDomainEvent domainEvent, CancellationToken ct)
    {
        await using AsyncServiceScope asyncScope = serviceScopeFactory.CreateAsyncScope();

        IServiceProvider serviceProvider = asyncScope.ServiceProvider;

        IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> handlers =
            serviceProvider.GetServices<IDomainEventHandlerScopedWrapper<TDomainEvent>>();

        await serviceProvider.GetRequiredService<IBackgroundExecutionStrategy>()
            .AsyncContinueOnException(handlers, domainEvent, ct);
    }

    public ValueTask PublishAsync(IDomainEvent domainEvent, CancellationToken ct)
    {
        return PublishAsync((TDomainEvent)domainEvent, ct);
    }
}
