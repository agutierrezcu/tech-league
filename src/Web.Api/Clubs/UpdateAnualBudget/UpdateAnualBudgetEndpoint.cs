using Application.Clubs.UpdateAnualBudget;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using IUpdateAnualBudgetCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Clubs.UpdateAnualBudget.UpdateAnualBudgetCommand>;

namespace Web.Api.Clubs.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetEndpoint
    (IUpdateAnualBudgetCommandHandler commandHandler)
        : Endpoint<UpdateAnualBudgetRequest>
{
    public override void Configure()
    {
        Patch("clubs/{clubId:Guid}/budget");
    }

    public override async Task HandleAsync(UpdateAnualBudgetRequest r, CancellationToken ct)
    {
        UpdateAnualBudgetCommand command = new(r.ClubId, r.NewAnualBudget);

        Result result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            () => Send.NoContentAsync(ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
