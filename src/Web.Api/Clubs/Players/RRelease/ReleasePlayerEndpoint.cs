using Application.Clubs.Players.RRelease;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using IReleasePlayerCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Clubs.Players.RRelease.ReleasePlayerCommand>;

namespace Web.Api.Clubs.Players.RRelease;

internal sealed class ReleasePlayerEndpoint
     (IReleasePlayerCommandHandler commandHandler)
        : Endpoint<ReleasePlayerRequest>
{
    public override void Configure()
    {
        Delete("clubs/{clubId:Guid}/players/{playerId:Guid}");
    }

    public override async Task HandleAsync(ReleasePlayerRequest r, CancellationToken ct)
    {
        ReleasePlayerCommand command = new(r.ClubId, r.PlayerId);

        Result result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            () => Send.NoContentAsync(ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
