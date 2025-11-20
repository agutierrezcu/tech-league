using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;
using Infrastructure.Database;

namespace Infrastructure.Entities.Clubs;

internal sealed class UpdateAnualBudgetProjectionRepository
    (TechLeagueDbContext dbContext)
        : IUpdateAnualBudgetProjectionRepository
{
    public async Task<ClubFinanceStatusProjection?> GetAsync(ClubId clubId, CancellationToken ct)
    {
        return await dbContext.FinanceStatusProjection.FindAsync([clubId], ct);
    }

    public Task SaveAsync(CancellationToken ct)
        => dbContext.SaveChangesAsync(ct);
}
