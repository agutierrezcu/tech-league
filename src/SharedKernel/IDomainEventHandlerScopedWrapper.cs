namespace SharedKernel;

public interface IDomainEventHandlerScopedWrapper<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken ct);

    Type DomainEventHandlerType { get; }
}
