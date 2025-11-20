using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.DomainEvents;
using SharedKernel;
using Errors = Application.Clubs.Projections.FinanceStatus.ClubFinanceStatusProjectionErrors;

namespace Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetProjectionCommandHandler
    (IUpdateAnualBudgetProjectionRepository repository, IEventBus eventBus)
        : ICommandHandler<UpdateAnualBudgetProjectionCommand>
{
    public async Task<Result> HandleAsync(UpdateAnualBudgetProjectionCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        ClubFinanceStatusProjection? projection = await repository.GetAsync(command.ClubId, ct);

        if (projection is null)
        {
            return Result.Failure(Errors.FinanceStatusProjectionNotFound);
        }

        projection.AnualBudget = projection.Club.AnualBudget;

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync([new FinanceStatusUpdatedDomainEvent()], ct);

        return Result.Success();
    }
}
