using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Coaches.Dismiss;
using Domain.DDD;
using SharedKernel;

namespace Application.Clubs.Coaches.Dismiss;

internal sealed class DismissCoachCommandHandler
    (IRepository<(ClubId ClubId, CoachId CoachId), DismissCoachAggregate, CoachContract, ContractId> repository,
        IEventBus eventBus)
            : ICommandHandler<DismissCoachCommand>
{
    public async Task<Result> HandleAsync(DismissCoachCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<DismissCoachAggregate> aggregateResult =
            await repository.GetAsync(new(command.ClubId, command.CoachId), ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure(aggregateResult.Error);
        }

        DismissCoachAggregate aggregate = aggregateResult.Value;

        Result result = aggregate.CancelContractAsync();

        if (result.IsFailure)
        {
            return result;
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success();
    }
}
