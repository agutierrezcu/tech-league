using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Clubs.UpdateBudget;
using Domain.DDD;
using SharedKernel;

namespace Application.Clubs.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetCommandHandler
    (IRepository<ClubId, UpdateAnualBudgetAggregate, Club, ClubId> repository, IEventBus eventBus)
        : ICommandHandler<UpdateAnualBudgetCommand>
{
    public async Task<Result> HandleAsync(UpdateAnualBudgetCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<UpdateAnualBudgetAggregate> aggregateResult = await repository.GetAsync(command.ClubId, ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure(aggregateResult.Error);
        }

        UpdateAnualBudgetAggregate aggregate = aggregateResult.Value;

        Result result = aggregate.Update(command.NewAnualBudget);

        if (result.IsFailure)
        {
            return result;
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success();
    }
}
