using SharedKernel;

namespace Domain.DomainEvents;

public sealed record CoachHiredDomainEvent(ContractId ContractId) : IDomainEvent;
