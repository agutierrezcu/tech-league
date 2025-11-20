using Application;

namespace Infrastructure.Entities;

internal sealed record ClubCommittedAnualBudgetInfo(Club Club, decimal CommittedAnualBudget);

internal interface IGetClubCommittedAnualBudgetQueryable : IQueryableDbContext
{
    Task<ClubCommittedAnualBudgetInfo?> GetAsync(ClubId clubId, CancellationToken ct);
}
