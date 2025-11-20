using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities.Clubs;

internal sealed class ClubFinanceStatusRepository
    (TechLeagueDbContext dbContext)
        : IClubFinanceStatusRepository
{
    public ValueTask<Club?> GetClubAsync(ClubId clubId, CancellationToken ct) 
        => dbContext.Clubs.FindAsync([clubId], ct);

    public async Task AddAsync(ClubFinanceStatusProjection projection, CancellationToken ct) 
        => await dbContext.FinanceStatusProjection.AddAsync(projection, ct);

    public ValueTask<Contract?> GetContractAsync(ContractId contractId, CancellationToken ct) 
        => dbContext.Contracts.FindAsync([contractId], ct);

    public Task<ClubFinanceStatusProjection?> GetProjectionAsync(ClubId clubId, CancellationToken ct)
    {
        return dbContext.FinanceStatusProjection
            .Include(p => p.Club)
            .FirstOrDefaultAsync(p => p.ClubId == clubId, ct);
    }

    public Task SaveAsync(CancellationToken ct) 
        => dbContext.SaveChangesAsync(ct);
}
