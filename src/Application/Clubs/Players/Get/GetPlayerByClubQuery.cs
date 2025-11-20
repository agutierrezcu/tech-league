using Application.Abstractions;
using Application.Abstractions.Messaging;

namespace Application.Clubs.Players.Get;

public sealed record GetPlayerByClubQuery(ClubId ClubId, string FilterByName,
    int PageIndex = 1, int PageSize = 10)
        : IQuery<PaginatedResult<PlayerByClub>>;
