using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.DomainEvents;
using SharedKernel;
using Errors = Application.Clubs.Projections.FinanceStatus.ClubFinanceStatusProjectionErrors;

namespace Application.Clubs.Projections.FinanceStatus.Update;

internal sealed class UpdateFinanceStatusProjectionCommandHandler
    (IUpdateFinanceStatusProjectionRepository repository, IEventBus eventBus)
        : ICommandHandler<UpdateFinanceStatusProjectionCommand>
{
    public async Task<Result> HandleAsync(UpdateFinanceStatusProjectionCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        ClubFinanceStatusProjection projection = await repository.GetAsync(command.ClubId, ct);

        if (projection is null)
        {
            return Result.Failure(Errors.FinanceStatusProjectionNotFound);
        }

        int adjustCountIn = command.UpdateIn > 0 ? 1 : -1;

        if (!ContractTypeExtensions.IsDefined(command.ContractType))
        {
            return Result.Failure(Errors.InvalidContractType);
        }

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

        if (projection.RemainingAnualBudget < 0)
        {
            return Result.Failure(Errors.InsufficientAnualBudget);
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync([new FinanceStatusUpdatedDomainEvent()], ct);

        return Result.Success();
    }
}
