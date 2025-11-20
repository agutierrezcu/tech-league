using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

internal sealed record ClubCommittedAnualBudget(Club Club, decimal CommittedAnualBudget);

internal static class ClubQueryExtensions
{
    internal static Task<ClubCommittedAnualBudget?> GetFinanceInfoByClubAsync(this IQueryable<Club> clubs,
        ClubId clubId, CancellationToken ct)
    {
        return clubs
            .Include(c => c.Contracts)
            .Where(c => c.Id == clubId)
            .Select(c =>
                new ClubCommittedAnualBudget(
                    c,
                    c.Contracts.Sum(c => c.AnualSalary)
                ))
            .FirstOrDefaultAsync(ct);
    }
}
