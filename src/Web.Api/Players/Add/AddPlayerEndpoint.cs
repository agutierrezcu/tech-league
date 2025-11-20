using System.Net;
using Application.Players.Add;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using IAddPlayerCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler
        <Application.Players.Add.AddPlayerCommand, Domain.Players.PlayerId>;

namespace Web.Api.Players.Add;

internal sealed class AddPlayerEndpoint
    (IAddPlayerCommandHandler commandHandler)
        : Endpoint<AddPlayerRequest>
{
    public override void Configure()
    {
        Post("players");
    }

    public override async Task HandleAsync(AddPlayerRequest r, CancellationToken ct)
    {
        AddPlayerCommand command = new(r.FullName, r.NickName, r.BirthDate);

        Result<PlayerId> result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            () =>
            {
                HttpContext.Response.Headers.Append("Location", $"/api/players/{result.Value}");
                return Send.ResponseAsync(null, (int)HttpStatusCode.Created, ct);
            },
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
