using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.Clubs;

internal sealed class UpdateAnualBudgetProjectionRepository
    (TechLeagueDbContext dbContext)
        : IUpdateAnualBudgetProjectionRepository
{
    public Task<ClubFinanceStatusProjection?> GetAsync(ClubId clubId, CancellationToken ct)
    {
        return dbContext.FinanceStatusProjection
            .Include(p => p.Club)
            .FirstOrDefaultAsync(p => p.ClubId == clubId, ct);
    }

    public Task SaveAsync(CancellationToken ct)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}
