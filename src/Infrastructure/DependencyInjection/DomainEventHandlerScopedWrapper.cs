using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure.DependencyInjection;

public sealed class DomainEventHandlerScopedWrapper<TEventHandler, TDomainEvent>
    (IServiceScopeFactory serviceScopeFactory)
        : IDomainEventHandlerScopedWrapper<TDomainEvent>
            where TEventHandler : IDomainEventHandler<TDomainEvent>
            where TDomainEvent : IDomainEvent
{
    public Type DomainEventHandlerType => typeof(TEventHandler);

    public async Task HandleAsync(TDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        await using AsyncServiceScope asyncScope = serviceScopeFactory.CreateAsyncScope();

        TEventHandler handler = asyncScope.ServiceProvider.GetRequiredService<TEventHandler>();

        await handler.HandleAsync(domainEvent, ct);
    }
}
