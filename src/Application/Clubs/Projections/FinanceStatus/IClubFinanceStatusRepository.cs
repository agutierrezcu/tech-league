using Domain.DDD;

namespace Application.Clubs.Projections.FinanceStatus;

public interface IClubFinanceStatusRepository : IRepository
{
    ValueTask<Club?> GetClubAsync(ClubId clubId, CancellationToken ct);
    
    Task AddAsync(ClubFinanceStatusProjection projection, CancellationToken ct);

    ValueTask<Contract?> GetContractAsync(ContractId contractId, CancellationToken ct);
    
    Task<ClubFinanceStatusProjection?> GetProjectionAsync(ClubId clubId, CancellationToken ct);
}
