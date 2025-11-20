using SharedKernel;

namespace Web.Api.Infrastructure.Background;

public interface IBackgroundExecutionStrategy
{
    Task AsyncContinueOnException<TDomainEvent>(
        IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> handlers,
            TDomainEvent domainEvent, CancellationToken ct)
       where TDomainEvent : IDomainEvent;
}
