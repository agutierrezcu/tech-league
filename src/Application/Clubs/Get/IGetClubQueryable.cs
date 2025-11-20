namespace Application.Clubs.Get;

public interface IGetClubQueryable : IQueryableDbContext
{
    Task<Club?> GetClubAsync(ClubId clubId, CancellationToken ct);
}
