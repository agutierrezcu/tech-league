
namespace Application.Clubs.Players.Get;

public interface IGetPlayersByClubQueryable : IQueryableDbContext
{
    IQueryable<Player> GetPlayersByClubAsync(ClubId clubId, CancellationToken ct);
}

