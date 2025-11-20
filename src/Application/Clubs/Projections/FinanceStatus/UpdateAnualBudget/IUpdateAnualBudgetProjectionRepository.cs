using Domain.DDD;

namespace Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;

public interface IUpdateAnualBudgetProjectionRepository : IRepository
{
    Task<ClubFinanceStatusProjection?> GetAsync(ClubId clubId, CancellationToken ct);
}
