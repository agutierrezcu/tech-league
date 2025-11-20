using Application.Clubs.Projections.FinanceStatus;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Web.Api.Infrastructure;

namespace Web.Api.Clubs.Finances.GetStatusByClub;

internal sealed class GetFinanceStatusByClubEndpoint
    (IClubFinanceStatusQueryable queryable)
        : Endpoint<GetFinanceStatusByClubRequest, GetClubsFinanceStatusResponse>
{
    public override void Configure()
    {
        Get("clubs/{clubId:Guid}/finance-status");
    }

    public override async Task HandleAsync(GetFinanceStatusByClubRequest r, CancellationToken ct)
    {
        GetClubsFinanceStatusResponse? response =
            await queryable.FinanceStatusProjections
                .Where(p => p.ClubId == r.ClubId)
                .Select(p => new GetClubsFinanceStatusResponse
                {
                    ClubId = p.ClubId,
                    ClubName = p.Club.Name,
                    AnualBudget = p.AnualBudget,
                    CommittedInPlayers = p.CommittedInPlayers,
                    CommittedInCoaches = p.CommittedInCoaches,
                    PlayerContractCount = p.PlayerContractCount,
                    CoachContractCount = p.CoachContractCount
                })
                .FirstOrDefaultAsync(ct);

        if (response is null)
        {
            var result = Result.Failure(ClubFinanceStatusProjectionErrors.ClubNotFound);
            
            await Send.ResultAsync(CustomResults.Problem(result));

            return;
        }

        await Send.OkAsync(response, ct);
    }
}
