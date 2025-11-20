using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Coaches.Add;
using Domain.DDD;
using SharedKernel;

namespace Application.Coaches.Add;

internal sealed class AddCoachCommandHandler
    (IRepository<CoachCreatorAggregate, Coach, CoachId> repository, IEventBus eventBus)
        : ICommandHandler<AddCoachCommand, CoachId>
{
    public async Task<Result<CoachId>> HandleAsync(AddCoachCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<CoachCreatorAggregate> aggregateResult = await repository.GetAsync(ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure<CoachId>(aggregateResult.Error);
        }

        CoachCreatorAggregate aggregate = aggregateResult.Value;

        Result result = aggregate.CreateCoach(command.FullName, command.Experience);

        if (result.IsFailure)
        {
            return Result.Failure<CoachId>(result.Error);
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success(aggregate.CoachId);
    }
}
