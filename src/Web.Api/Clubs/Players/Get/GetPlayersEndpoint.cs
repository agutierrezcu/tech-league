using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Clubs.Players.Get;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Clubs.Players.Get;

internal sealed class GetPlayersEndpoint
     (IQueryHandler<GetPlayerByClubQuery, PaginatedResult<PlayerByClub>> queryHandler)
        : Endpoint<GetPlayersRequest, PlayerByClub[]>
{
    public override void Configure()
    {
        Get("/clubs/{clubId:Guid}/players");
    }

    public override async Task HandleAsync(GetPlayersRequest r, CancellationToken ct)
    {
        GetPlayerByClubQuery query = new(r.ClubId, r.FilterByName, r.PageIndex, r.PageSize);

        Result<PaginatedResult<PlayerByClub>> result = await queryHandler.HandleAsync(query, ct);

        await result.Match(
            v =>
            {
                HttpContext.Response.Headers.AddPaginationHeaders(v);
                return Send.OkAsync(v.Data, ct);
            },
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
