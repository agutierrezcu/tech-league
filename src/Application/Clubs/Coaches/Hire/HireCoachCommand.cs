using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Clubs.Coaches.Hire;

public sealed record HireCoachCommand(ClubId ClubId, CoachId CoachId,
    DateRange ContractDuration, decimal AnualSalary) : ICommand;
