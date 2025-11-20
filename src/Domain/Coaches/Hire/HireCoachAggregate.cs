using Domain.DDD;
using Domain.DomainEvents;
using Domain.ValueObjects;
using SharedKernel;
using static Domain.Coaches.Hire.HireCoachAggregateErrors;

namespace Domain.Coaches.Hire;

public class HireCoachAggregateRoot(Club club, Coach coach)
{
    public Club Club => club;

    public Coach Coach => coach;

    public CoachContract CoachContract { get; set; }
}

public sealed class HireCoachAggregate
    (HireCoachAggregateRoot root, decimal committedAnualBudget)
        : AggregateRoot<HireCoachAggregateRoot>
{
    protected override HireCoachAggregateRoot Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public Result HireCoach(DateRange contractDuration, decimal anualSalary)
    {
        ArgumentNullException.ThrowIfNull(contractDuration);

        if (Root.Coach.IsCoachingTo(Root.Club.Id))
        {
            return Result.Failure<HireCoachAggregate>(CoachAlreadyHired);
        }

        if (!Root.Coach.IsAvailable)
        {
            return Result.Failure<HireCoachAggregate>(CoachWorkingForDifferentClub);
        }

        decimal remainingBudget = root.Club.AnualBudget - committedAnualBudget;

        if (anualSalary > remainingBudget)
        {
            return Result.Failure(InsufficientRemainingBudget);
        }

        Root.CoachContract = new()
        {
            Club = Root.Club,
            Coach = Root.Coach,
            Duration = contractDuration,
            AnualSalary = anualSalary
        };

        RegisterEvent(() => new CoachHiredDomainEvent(Root.CoachContract.Id));

        return Result.Success();
    }
}
