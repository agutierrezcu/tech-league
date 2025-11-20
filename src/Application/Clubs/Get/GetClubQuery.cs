using Application.Abstractions.Messaging;

namespace Application.Clubs.Get;

public sealed record GetClubQuery(ClubId ClubId) : IQuery<GetClubQueryResult>;
