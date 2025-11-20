using Domain.DDD;
using Domain.DomainEvents;
using Domain.ValueObjects;
using SharedKernel;

using Errors = Domain.Players.SignUp.SignUpPlayerAggregateErrors;

namespace Domain.Players.SignUp;

public sealed class SignUpPlayerAggregate
    (Player root, (ClubId ClubId, decimal AnualBudget, decimal CommittedAnualBudget) clubInfo)
        : AggregateRoot<Player, PlayerId>
{
    protected override Player Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public Result SignUp(DateRange contractDuration, decimal anualSalary,
        DateRange signingWindow, IDateTimeProvider dateTimeProvider)
    {
        ArgumentNullException.ThrowIfNull(contractDuration);

        if (!signingWindow.Contains(dateTimeProvider.UtcNow))
        {
            return Result.Failure(Errors.OutOfSigningWindow);
        }

        if (Root.IsPlayingFor(clubInfo.ClubId))
        {
            return Result.Failure(Errors.PlayedAlreadySigned);
        }

        if (!Root.IsFreeAgent)
        {
            return Result.Failure(Errors.PlayerUnderContractWithDifferentClub);
        }

        decimal remainingBudget = clubInfo.AnualBudget - clubInfo.CommittedAnualBudget;

        if (anualSalary > remainingBudget)
        {
            return Result.Failure(Errors.InsufficientRemainingBudget);
        }

        Root.CurrentContract = new()
        {
            ClubId = clubInfo.ClubId,
            Duration = contractDuration,
            AnualSalary = anualSalary
        };


        RegisterEvent(() => new PlayerSignedUpDomainEvent(Root.CurrentContract.Id));

        return Result.Success();
    }
}
