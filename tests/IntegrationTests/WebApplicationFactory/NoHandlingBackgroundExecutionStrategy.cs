using SharedKernel;
using Shouldly;
using Web.Api.Infrastructure.Background;

namespace IntegrationTests.WebApplicationFactory;

public sealed class NoHandlingBackgroundExecutionStrategy
    (IBackgroundExecutionStrategy inner)
        : IBackgroundExecutionStrategy
{
    public IEnumerable<Type> HandlerTypes { get; private set; }

    public IDomainEvent DomainEvent { get; private set; }

    public Task AsyncContinueOnException<TDomainEvent>(
        IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> handlers,
            TDomainEvent domainEvent, CancellationToken ct)
                where TDomainEvent : IDomainEvent
    {
        inner.ShouldNotBeNull()
            .ShouldBeOfType<BackgroundExecutionInParallerlStrategy>();

        HandlerTypes = handlers
            .Select(h => (Type)h.GetType().GetProperty("DomainEventHandlerType")!.GetValue(h))
            .ToList()!;

        DomainEvent = domainEvent;

        return Task.CompletedTask;
    }
}
