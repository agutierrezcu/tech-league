using SharedKernel;

namespace Domain.DomainEvents;

public sealed record CoachDismissedDomainEvent
    (ClubId ClubId, decimal AnualSalary)
        : IDomainEvent;
