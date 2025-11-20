using SharedKernel;

namespace Domain.DomainEvents;

public sealed record PlayerSignedUpDomainEvent(ContractId ContractId) : IDomainEvent;
