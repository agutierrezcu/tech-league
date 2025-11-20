using FluentAssertions;
using Infrastructure.EventBus;
using SharedKernel;

namespace IntegrationTests.WebApplicationFactory;

public sealed class NoHandlingExecutionStrategy(IExecutionStrategy inner)
    : IExecutionStrategy
{
    public IEnumerable<Type> HandlerTypes { get; private set; }

    public IDomainEvent DomainEvent { get; private set; }

    public Task AsyncContinueOnException<TDomainEvent>(
        IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> handlers,
            TDomainEvent domainEvent, CancellationToken ct)
                where TDomainEvent : IDomainEvent
    {
        inner.Should().NotBeNull()
            .And
            .BeOfType<ExecutionInParallerlStrategy>();

        HandlerTypes = handlers
            .Select(h => (Type)h.GetType().GetProperty("DomainEventHandlerType")!.GetValue(h))
            .ToList()!;

        DomainEvent = domainEvent;

        return Task.CompletedTask;
    }
}
