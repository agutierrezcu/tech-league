namespace Application.Clubs.Projections;

public interface IClubFinanceStatusQueryable : IQueryableDbContext
{
    Task<Contract?> GetContractAsync(ContractId contractId, CancellationToken ct);

    IQueryable<ClubFinanceStatusProjection> GetAllFinanceStatusProjections();
}
