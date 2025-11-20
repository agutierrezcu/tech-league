using Application.Abstractions.Messaging;
using SharedKernel;

using static Application.Clubs.Projections.FinanceStatus.ClubFinanceStatusProjectionErrors;

namespace Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetProjectionCommandHandler
    (IClubFinanceStatusRepository repository)
        : ICommandHandler<UpdateAnualBudgetFinanceStatusProjectionCommand>
{
    public async Task<Result> HandleAsync(UpdateAnualBudgetFinanceStatusProjectionCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        ClubFinanceStatusProjection? projection =
            await repository.GetProjectionAsync(command.ClubId, ct);

        if (projection is null)
        {
            return Result.Failure(FinanceStatusProjectionNotFound);
        }

        projection.AnualBudget = projection.Club.AnualBudget;

        await repository.SaveAsync(ct);

        return Result.Success();
    }
}
