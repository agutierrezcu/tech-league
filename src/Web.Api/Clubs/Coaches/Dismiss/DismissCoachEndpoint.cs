using Application.Clubs.Coaches.Dismiss;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using IDismissCoachCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Clubs.Coaches.Dismiss.DismissCoachCommand>;

namespace Web.Api.Clubs.Coaches.Dismiss;

internal sealed class DismissCoachEndpoint
    (IDismissCoachCommandHandler commandHandler)
        : Endpoint<DismissCoachRequest>
{
    public override void Configure()
    {
        Delete("clubs/{clubId:Guid}/coaches/{coachId:Guid}");
    }

    public override async Task HandleAsync(DismissCoachRequest r, CancellationToken ct)
    {
        DismissCoachCommand command = new(r.ClubId, r.CoachId);

        Result result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            () => Send.NoContentAsync(ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
