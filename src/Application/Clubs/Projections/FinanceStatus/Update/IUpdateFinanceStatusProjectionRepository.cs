using Domain.DDD;

namespace Application.Clubs.Projections.FinanceStatus.Update;

public interface IUpdateFinanceStatusProjectionRepository : IRepository
{
    Task<ClubFinanceStatusProjection?> GetAsync(ClubId clubId, CancellationToken ct);
}
