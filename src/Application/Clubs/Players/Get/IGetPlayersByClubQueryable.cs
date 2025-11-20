using Application.Abstractions;

namespace Application.Clubs.Players.Get;

public interface IGetPlayersByClubQueryable : IQueryableDbContext
{
    Task<PaginatedResult<PlayerByClub>?> GetPlayersByClubAsync(ClubId clubId,
        string? filterByFullName, int pageIndex, int pageSize, CancellationToken ct);
}
