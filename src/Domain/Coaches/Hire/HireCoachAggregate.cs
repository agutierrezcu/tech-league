using Domain.DDD;
using Domain.DomainEvents;
using Domain.ValueObjects;
using SharedKernel;

using Errors = Domain.Coaches.Hire.HireCoachAggregateErrors;

namespace Domain.Coaches.Hire;

public sealed class HireCoachAggregate
    (Coach root, (ClubId ClubId, decimal AnualBudget, decimal CommittedAnualBudget) clubInfo)
        : AggregateRoot<Coach, CoachId>
{
    protected override Coach Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public Result HireCoach(DateRange contractDuration, decimal anualSalary)
    {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(contractDuration);

        if (Root.IsCoachingTo(clubInfo.ClubId))
        {
            return Result.Failure<HireCoachAggregate>(Errors.CoachAlreadyHired);
        }

        if (!Root.IsAvailable)
        {
            return Result.Failure<HireCoachAggregate>(Errors.CoachWorkingForDifferentClub);
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

        RegisterEvent(() => new CoachHiredDomainEvent(Root.CurrentContract.Id));

        return Result.Success();
    }
}
