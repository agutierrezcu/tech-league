using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using SharedKernel;

namespace Application.Clubs.Projections.FinanceStatus.GetAll;

internal sealed class GetAllFinanceStatusQueryHandler
    (HybridCache hybridCache, IClubFinanceStatusQueryable queryable)
       : IQueryHandler<GetAllFinanceStatusQuery, FinanceStatus[]>
{
    public Task<Result<FinanceStatus[]>> HandleAsync(GetAllFinanceStatusQuery _,
        CancellationToken ct)
    {
        return hybridCache.GetOrCreateAsync(
           "all-finance-status",
           async token => await InternalQueryHandler(token),
           tags: ["finance-status"],
           cancellationToken: ct
       ).AsTask();
    }

    private async Task<Result<FinanceStatus[]>> InternalQueryHandler(CancellationToken ct)
    {
        FinanceStatus[] response =
            await queryable.GetAllFinanceStatusProjections()
                .Select(p => new FinanceStatus
                {
                    ClubId = p.ClubId,
                    ClubName = p.Club.Name,
                    AnualBudget = p.AnualBudget,
                    CommittedInPlayers = p.CommittedInPlayers,
                    CommittedInCoaches = p.CommittedInCoaches,
                    PlayerContractCount = p.PlayerContractCount,
                    CoachContractCount = p.CoachContractCount,
                    RemainingAnualBudget = p.RemainingAnualBudget,
                    CommittedAnualBudget = p.CommittedAnualBudget,
                    TotalContractCount = p.TotalContractsCount
                })
                .ToArrayAsync(ct);

        return Result.Success(response);
    }
}
