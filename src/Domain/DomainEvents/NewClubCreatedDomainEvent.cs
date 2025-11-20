using SharedKernel;

namespace Domain.DomainEvents;

public sealed record NewClubCreatedDomainEvent(ClubId ClubId) : IDomainEvent;
