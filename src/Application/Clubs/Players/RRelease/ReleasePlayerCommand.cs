using Application.Abstractions.Messaging;

namespace Application.Clubs.Players.RRelease;

public sealed record ReleasePlayerCommand(ClubId ClubId, PlayerId PlayerId)
    : ICommand;
