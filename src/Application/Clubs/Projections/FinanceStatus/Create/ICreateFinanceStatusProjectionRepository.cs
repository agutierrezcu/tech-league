using Domain.DDD;

namespace Application.Clubs.Projections.FinanceStatus.Create;

public interface ICreateFinanceStatusProjectionRepository : IRepository
{
    Task AddAsync(ClubFinanceStatusProjection projection, CancellationToken ct);
}
