using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus.Create;
using Infrastructure.Database;

namespace Infrastructure.Entities.Clubs;

internal sealed class CreateFinanceStatusProjectionRepository
    (TechLeagueDbContext dbContext)
        : ICreateFinanceStatusProjectionRepository
{
    public async Task AddAsync(ClubFinanceStatusProjection projection, CancellationToken ct)
    {
        await dbContext.FinanceStatusProjection.AddAsync(projection, ct);
    }

    public Task SaveAsync(CancellationToken ct)
        => dbContext.SaveChangesAsync(ct);
}
