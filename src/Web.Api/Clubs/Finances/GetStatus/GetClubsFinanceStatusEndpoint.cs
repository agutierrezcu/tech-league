using Application.Clubs.Projections.FinanceStatus;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Clubs.Finances.GetStatus;

internal sealed class GetClubsFinanceStatusEndpoint
    (IClubFinanceStatusQueryable queryable)
        : EndpointWithoutRequest<GetClubsFinanceStatusResponse[]>
{
    public override void Configure()
    {
        Get("clubs/finance-status");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        GetClubsFinanceStatusResponse[] viewModel =
            await queryable.FinanceStatusProjections
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
                .ToArrayAsync(ct);

        await Send.OkAsync(viewModel, ct);
    }
}
