using Domain.DDD;
using Domain.DomainEvents;
using SharedKernel;

using static Domain.Clubs.UpdateBudget.UpdateAnualBudgetAggregateErrors;

namespace Domain.Clubs.UpdateBudget;

public sealed class UpdateAnualBudgetAggregate(Club root, decimal committedBudget)
    : AggregateRoot<Club, ClubId>
{
    protected override Club Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public Result Update(decimal newAnualBudget)
    {
        if (newAnualBudget < Club.MinimumAnualBudget)
        {
            return Result.Failure(ValueBelowMinimumRequired);
        }

        if (newAnualBudget < committedBudget)
        {
            return Result.Failure(ValueBellowCommittedBudget);
        }

        Root.AnualBudget = newAnualBudget;

        RegisterEvent(() => new ClubAnualBudgetUpdatedDomainEvent(Root.Id));

        return Result.Success();
    }
}
