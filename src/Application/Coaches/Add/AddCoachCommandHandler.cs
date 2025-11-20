using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Coaches.Add;
using Domain.DDD;
using SharedKernel;

namespace Application.Coaches.Add;

internal sealed class AddCoachCommandHandler
    (IRepository<CoachCreatorAggregate, Coach> repository, IEventBus eventBus)
        : ICommandHandler<AddCoachCommand, CoachId>
{
    public async Task<Result<CoachId>> HandleAsync(AddCoachCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        CoachCreatorAggregate aggregate = await repository.GetAsync(ct);

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
