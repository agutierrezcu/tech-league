using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Coaches.Hire;
using Domain.DDD;
using SharedKernel;

namespace Application.Clubs.Coaches.Hire;

internal sealed class HireCoachCommandHandler
    (IRepository<(ClubId ClubId, CoachId CoachId), HireCoachAggregate, Coach, CoachId> repository,
        IEventBus eventBus)
            : ICommandHandler<HireCoachCommand>
{
    public async Task<Result> HandleAsync(HireCoachCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<HireCoachAggregate> aggregateResult =
            await repository.GetAsync((command.ClubId, command.CoachId), ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure(aggregateResult.Error);
        }

        HireCoachAggregate aggregate = aggregateResult.Value;

        Result result = aggregate.HireCoach(command.ContractDuration, command.AnualSalary);

        if (result.IsFailure)
        {
            return result;
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success();
    }
}
