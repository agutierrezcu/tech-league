using Application.Abstractions.Messaging;
using SharedKernel;

using static Application.Clubs.Projections.FinanceStatus.ClubFinanceStatusProjectionErrors;

namespace Application.Clubs.Projections.FinanceStatus.Update;

internal sealed class UpdateFinanceStatusProjectionCommandHandler
    (IClubFinanceStatusRepository repository)
        : ICommandHandler<UpdateFinanceStatusProjectionCommand>
{
    public async Task<Result> HandleAsync(UpdateFinanceStatusProjectionCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        ClubFinanceStatusProjection projection =
            await repository.GetProjectionAsync(command.ClubId, ct);

        if (projection is null)
        {
            return Result.Failure(FinanceStatusProjectionNotFound);
        }

        int adjustCountIn = command.UpdateIn > 0 ? 1 : -1;

        if (command.ContractType == ContractType.Player)
        {
            projection.CommittedInPlayers += command.UpdateIn;
            projection.PlayerContractCount += adjustCountIn;
        }
        else if (command.ContractType == ContractType.Coach)
        {
            projection.CommittedInCoaches += command.UpdateIn;
            projection.CoachContractCount += adjustCountIn;
        }
        else
        {
            return Result.Failure(InvalidContractType);
        }

        if (projection.RemainingAnualBudget < 0)
        {
            return Result.Failure(InsufficientAnualBudget);
        }

        await repository.SaveAsync(ct);

        return Result.Success();
    }
}
