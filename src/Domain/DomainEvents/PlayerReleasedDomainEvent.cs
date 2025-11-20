using SharedKernel;

namespace Domain.DomainEvents;

public sealed record PlayerReleasedDomainEvent
    (ClubId ClubId, decimal AnualSalary)
        : IDomainEvent;
