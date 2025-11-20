using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.DDD;
using Domain.Players.Add;
using SharedKernel;

namespace Application.Players.Add;

internal sealed class AddPlayerCommandHandler
    (IRepository<PlayerCreatorAggregate, Player, PlayerId> repository, IEventBus eventBus)
        : ICommandHandler<AddPlayerCommand, PlayerId>
{
    public async Task<Result<PlayerId>> HandleAsync(AddPlayerCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<PlayerCreatorAggregate> aggregateResult = await repository.GetAsync(ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure<PlayerId>(aggregateResult.Error);
        }

        PlayerCreatorAggregate aggregate = aggregateResult.Value;

        Result result = aggregate.CreatePlayer(command.FullName, command.NickName, command.BirthDate);

        if (result.IsFailure)
        {
            return Result.Failure<PlayerId>(result.Error);
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success(aggregate.PlayerId);
    }
}
