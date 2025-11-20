using SharedKernel;

namespace Infrastructure.EventBus;

public interface IExecutionStrategy
{
    Task AsyncContinueOnException<TDomainEvent>(
        IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> handlers,
            TDomainEvent domainEvent, CancellationToken ct)
       where TDomainEvent : IDomainEvent;
}
