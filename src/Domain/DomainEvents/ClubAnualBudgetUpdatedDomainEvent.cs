using SharedKernel;

namespace Domain.DomainEvents;

public sealed record ClubAnualBudgetUpdatedDomainEvent(ClubId ClubId) : IDomainEvent;
