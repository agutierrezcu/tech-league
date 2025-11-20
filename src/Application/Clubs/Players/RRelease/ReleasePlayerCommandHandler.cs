using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.DDD;
using Domain.Players.RRelease;
using SharedKernel;

namespace Application.Clubs.Players.RRelease;

internal sealed class ReleasePlayerCommandHandler
    (IRepository<(ClubId ClubId, PlayerId PlayerId), ReleasePlayerAggregate, PlayerContract, ContractId> repository,
        IEventBus eventBus)
            : ICommandHandler<ReleasePlayerCommand>
{
    public async Task<Result> HandleAsync(ReleasePlayerCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<ReleasePlayerAggregate> aggregateResult = await repository.GetAsync(
            (command.ClubId, command.PlayerId), ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure(aggregateResult.Error);
        }

        ReleasePlayerAggregate aggregate = aggregateResult.Value;

        Result result = await aggregate.ReleasePlayer();

        if (result.IsFailure)
        {
            return result;
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success();
    }
}
