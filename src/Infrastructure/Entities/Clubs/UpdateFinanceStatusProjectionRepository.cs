using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus.Update;
using Infrastructure.Database;

namespace Infrastructure.Entities.Clubs;

internal sealed class UpdateFinanceStatusProjectionRepository
    (TechLeagueDbContext dbContext)
        : IUpdateFinanceStatusProjectionRepository
{
    public async Task<ClubFinanceStatusProjection?> GetAsync(ClubId clubId, CancellationToken ct)
    {
        return await dbContext.FinanceStatusProjection.FindAsync([clubId], ct);
    }

    public async Task SaveAsync(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}
