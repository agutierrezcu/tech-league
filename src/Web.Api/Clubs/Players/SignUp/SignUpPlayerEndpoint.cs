using Application.Clubs.Players.SignUp;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using ISignUpPlayerCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Clubs.Players.SignUp.SignUpPlayerCommand>;

namespace Web.Api.Clubs.Players.SignUp;

internal sealed class SignUpPlayerEndpoint
    (ISignUpPlayerCommandHandler commandHandler)
        : Endpoint<SignUpPlayerRequest>
{
    public override void Configure()
    {
        Post("clubs/{clubId:Guid}/players");
    }

    public override async Task HandleAsync(SignUpPlayerRequest r, CancellationToken ct)
    {
        SignUpPlayerCommand command = new(r.ClubId, r.PlayerId,
            r.ContractDuration, r.AnualSalary);

        Result result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            () => Send.NoContentAsync(ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
