using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

internal static class ClubQueryExtensions
{
    internal static Task<ClubCommittedAnualBudgetInfo?> GetFinanceInfoByClubAsync(this IQueryable<Club> clubs,
        ClubId clubId, CancellationToken ct)
    {
        return clubs
            .Include(c => c.Contracts)
            .Where(c => c.Id == clubId)
            .TagWith($"{nameof(GetFinanceInfoByClubAsync)}")
            .Select(c => 
                new ClubCommittedAnualBudgetInfo(c, c.Contracts.Sum(c => c.AnualSalary)))
            .FirstOrDefaultAsync(ct);
    }
}
