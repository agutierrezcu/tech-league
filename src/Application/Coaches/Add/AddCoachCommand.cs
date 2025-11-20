using Application.Abstractions.Messaging;

namespace Application.Coaches.Add;

public sealed record AddCoachCommand(string FullName, int Experience)
    : ICommand<CoachId>;
