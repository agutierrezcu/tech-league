using Application.Abstractions.Messaging;
using Application.Clubs.Get;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Clubs.Get;

internal sealed class GetClubEndpoint
    (IQueryHandler<GetClubQuery, GetClubQueryResult> queryHandler)
        : Endpoint<GetClubRequest, GetClubResponse>
{
    public override void Configure()
    {
        Get("clubs/{clubId:Guid}");
    }

    public override async Task HandleAsync(GetClubRequest r, CancellationToken ct)
    {
        GetClubQuery query = new(r.ClubId);

        Result<GetClubQueryResult> result = await queryHandler.HandleAsync(query, ct);

        await result.Match(
            value =>
                Send.OkAsync(
                    new(value.Name, value.ThreeLettersName, value.AnualBudget),
                    cancellation: ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
