using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Clubs.Players.SignUp.SigningWindow;
using Domain.DDD;
using Domain.Players.SignUp;
using Domain.ValueObjects;
using SharedKernel;

namespace Application.Clubs.Players.SignUp;

internal sealed class SignUpPlayerCommandHandler
    (IRepository<(ClubId ClubId, PlayerId PlayerId), SignUpPlayerAggregate, Player, PlayerId> repository,
        SigningWindowSetting signingWindow, IDateTimeProvider dateTimeProvider, IEventBus eventBus)
            : ICommandHandler<SignUpPlayerCommand>
{
    public async Task<Result> HandleAsync(SignUpPlayerCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Result<SignUpPlayerAggregate> aggregateResult = await repository.GetAsync(
            (command.ClubId, command.PlayerId), ct);

        if (aggregateResult.IsFailure)
        {
            return Result.Failure(aggregateResult.Error);
        }

        SignUpPlayerAggregate aggregate = aggregateResult.Value;

        Result<DateRange> signingDateRangeResult =
            DateRange.Create(signingWindow.Start, signingWindow.End);

        if (signingDateRangeResult.IsFailure)
        {
            return signingDateRangeResult;
        }

        Result result = aggregate.SignUp(command.ContractDuration,
            command.AnualSalary, signingDateRangeResult.Value, dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync(aggregate, ct);

        return Result.Success();
    }
}
