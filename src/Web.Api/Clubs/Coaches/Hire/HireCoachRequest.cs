using Domain.ValueObjects;

namespace Web.Api.Clubs.Coaches.Hire;

internal sealed record HireCoachRequest(ClubId ClubId, CoachId CoachId,
    DateRange ContractDuration, decimal AnualSalary);
