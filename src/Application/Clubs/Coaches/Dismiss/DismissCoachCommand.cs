using Application.Abstractions.Messaging;

namespace Application.Clubs.Coaches.Dismiss;

public sealed record DismissCoachCommand(ClubId ClubId, CoachId CoachId)
    : ICommand;
