using FastEndpoints;

namespace Web.Api.Clubs.Players.Get;

internal sealed record GetPlayersRequest(ClubId ClubId, string FilterByName,
    int PageSize = 10)
{
    [BindFrom("page")]
    public int PageIndex { get; init; } = 1;
}
