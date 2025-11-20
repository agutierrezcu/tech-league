using Application.Clubs.Coaches.Hire;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using IHireCoachCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Clubs.Coaches.Hire.HireCoachCommand>;

namespace Web.Api.Clubs.Coaches.Hire;

internal sealed class HireCoachEndpoint
     (IHireCoachCommandHandler commandHandler)
        : Endpoint<HireCoachRequest>
{
    public override void Configure()
    {
        Post("clubs/{clubId:Guid}/coaches");
    }

    public override async Task HandleAsync(HireCoachRequest r, CancellationToken ct)
    {
        HireCoachCommand command = new(r.ClubId, r.CoachId,
            r.ContractDuration, r.AnualSalary);

        Result result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            () => Send.NoContentAsync(ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
