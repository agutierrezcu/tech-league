using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Clubs.Players.SignUp;

public sealed record SignUpPlayerCommand(ClubId ClubId, PlayerId PlayerId,
    DateRange ContractDuration, decimal AnualSalary) : ICommand;
