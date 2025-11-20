using Application.Abstractions;

namespace Application.Clubs.Players.Get;

public sealed record PlayerByClub(PlayerId Id, string FullName, string? NickName, DateOnly? BirthDate);

public sealed record GetPlayerByClubQueryResponse : PaginationInfo<PlayerByClub>
{
    public GetPlayerByClubQueryResponse(PlayerByClub[] Data, int TotalCount, 
        int PageIndex, int PageSize, int TotalPages) 
            : base(Data, TotalCount, PageIndex, PageSize, TotalPages)
    {
    }
}
