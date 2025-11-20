using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Clubs.Add;
using Domain.DDD;
using SharedKernel;

namespace Application.Clubs.Add;

internal sealed class AddClubCommandHandler
    (IRepository<(string Name, string ThreeLettersName), ClubCreatorAggregate, Club, ClubId> repository,
        IEventBus eventBus)
            : ICommandHandler<AddClubCommand, ClubId>
{
    public async Task<Result<ClubId>> HandleAsync(AddClubCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<ClubCreatorAggregate> aggregateResult = await repository.GetAsync(
            (command.Name, command.ThreeLettersName), ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure<ClubId>(aggregateResult.Error);
        }

        ClubCreatorAggregate aggregate = aggregateResult.Value;

        Result result = aggregate.CreateClub(command.AnualBudget);

        if (result.IsFailure)
        {
            return Result.Failure<ClubId>(result.Error);
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success(aggregate.ClubId);
    }
}
