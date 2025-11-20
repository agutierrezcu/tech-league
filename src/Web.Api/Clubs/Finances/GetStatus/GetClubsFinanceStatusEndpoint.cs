using Application.Abstractions.Messaging;
using Application.Clubs.Projections.FinanceStatus.GetAll;
using FastEndpoints;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Clubs.Finances.GetStatus;

internal sealed class GetClubsFinanceStatusEndpoint
    (IQueryHandler<GetAllFinanceStatusQuery, FinanceStatus[]> queryHanlder)
        : EndpointWithoutRequest<FinanceStatus[]>
{
    public override void Configure()
    {
        Get("clubs/finance-status");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Result<FinanceStatus[]> result = await queryHanlder.HandleAsync(new(), ct);

        await result.Match(
            v => Send.OkAsync(v, cancellation: ct),
            r => Send.ResultAsync(CustomResults.Problem(r))
        );
    }
}
