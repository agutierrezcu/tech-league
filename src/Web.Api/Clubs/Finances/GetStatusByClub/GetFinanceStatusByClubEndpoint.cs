using Application.Clubs.Projections;
using Application.Clubs.Projections.FinanceStatus.GetAll;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Web.Api.Infrastructure;
using Errors = Application.Clubs.Projections.FinanceStatus.ClubFinanceStatusProjectionErrors;

namespace Web.Api.Clubs.Finances.GetStatusByClub;

internal sealed class GetFinanceStatusByClubEndpoint
    (IClubFinanceStatusQueryable queryable)
        : Endpoint<GetFinanceStatusByClubRequest, FinanceStatus>
{
    public override void Configure()
    {
        Get("clubs/{clubId:Guid}/finance-status");
    }

    public override async Task HandleAsync(GetFinanceStatusByClubRequest r, CancellationToken ct)
    {
        FinanceStatus? response =
            await queryable.GetAllFinanceStatusProjections()
                .Where(p => p.ClubId == r.ClubId)
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
                .FirstOrDefaultAsync(ct);

        if (response is null)
        {
            var result = Result.Failure(Errors.ClubNotFound);

            await Send.ResultAsync(CustomResults.Problem(result));

            return;
        }

        await Send.OkAsync(response, ct);
    }
}
