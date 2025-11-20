using Application.Clubs.Get;
using Application.Clubs.Players.Get;
using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

internal sealed class TechLeagueQueryableDbContext
    : QueryableDbContext<TechLeagueDbContext>,
        IGetClubQueryable,
        IGetPlayersByClubQueryable,
        IClubFinanceStatusQueryable
{
    public TechLeagueQueryableDbContext(DbContextOptions<TechLeagueDbContext> options)
        : base(options)
    {
    }

    public IQueryable<ClubFinanceStatusProjection> FinanceStatusProjections 
        => AsNoTracking<ClubFinanceStatusProjection>();

    public IQueryable<Player> GetPlayersByClubAsync(ClubId clubId, CancellationToken ct)
    {
        return AsNoTrackingWithIdentityResolution<Player>()
            .Where(c => c.CurrentContract!.ClubId == clubId);
    }

    public Task<Club?> GetClubAsync(ClubId clubId, CancellationToken ct)
    {
        return AsNoTrackingWithIdentityResolution<Club>()
            .FirstOrDefaultAsync(c => c.Id == clubId, ct);
    }
}
