using Application.Clubs.Add;
using FastEndpoints;
using SharedKernel;
using Web.Api.Clubs.Get;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using IAddClubCommandCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Clubs.Add.AddClubCommand, Domain.Clubs.ClubId>;

namespace Web.Api.Clubs.Add;

internal sealed class AddClubEndpoint
    (IAddClubCommandCommandHandler commandHandler)
        : Endpoint<AddClubRequest>
{
    public override void Configure()
    {
        Post("clubs");
    }

    public override async Task HandleAsync(AddClubRequest r, CancellationToken ct)
    {
        AddClubCommand command = new(r.Name, r.ThreeLettersName, r.AnualBudget);

        Result<ClubId> result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            value =>
                Send.CreatedAtAsync<GetClubEndpoint>(
                    new { clubId = value },
                    cancellation: ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
