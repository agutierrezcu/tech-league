using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus.Create;
using Infrastructure.Database;

namespace Infrastructure.Entities.Clubs;

internal sealed class CreateFinanceStatusProjectionRepository
    (TechLeagueDbContext dbContext)
        : ICreateFinanceStatusProjectionRepository
{
    public Task AddAsync(ClubFinanceStatusProjection projection, CancellationToken ct)
    {
         return dbContext.FinanceStatusProjection.AddAsync(projection, ct).AsTask();
    }

    public Task SaveAsync(CancellationToken ct)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}
