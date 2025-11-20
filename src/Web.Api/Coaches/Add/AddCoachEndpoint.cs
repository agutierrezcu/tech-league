using System.Net;
using Application.Coaches.Add;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using IAddCoachCommandHandler =
    Application.Abstractions.Messaging.ICommandHandler<
        Application.Coaches.Add.AddCoachCommand, Domain.Coaches.CoachId>;

namespace Web.Api.Coaches.Add;

internal sealed class AddCoachEndpoint
    (IAddCoachCommandHandler commandHandler)
        : Endpoint<AddCoachRequest>
{
    public override void Configure()
    {
        Post("coaches");
    }

    public override async Task HandleAsync(AddCoachRequest r, CancellationToken ct)
    {
        AddCoachCommand command = new(r.FullName, r.Experience);

        Result<CoachId> result = await commandHandler.HandleAsync(command, ct);

        await result.Match(
            v =>
            {
                HttpContext.Response.Headers.Append("Location", $"/api/coaches/{v}");
                return Send.ResponseAsync(null, (int)HttpStatusCode.Created, ct);
            },
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
